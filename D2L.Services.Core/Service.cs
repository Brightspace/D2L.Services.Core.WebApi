using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Dependencies;
using D2L.Security.OAuth2.Principal;
using D2L.Security.OAuth2.Validation.Request;
using D2L.Services.Core.Activation;
using D2L.Services.Core.Configuration;
using Microsoft.Owin.Hosting;
using Owin;
using SimpleLogInterface;

namespace D2L.Services.Core {
	internal sealed class Service : IService {
		private readonly ServiceDescriptor m_serviceDescriptor;
		private readonly IConfigViewer m_configViewer;
		private readonly ILogProvider m_logProvider;
		private readonly IDependencyResolver m_dependencyResolver;
		private readonly Action<HttpConfiguration> m_startup;

		private IDisposable m_disposeHandle;

		public Service(
			ServiceDescriptor serviceDescriptor,
			IConfigViewer configViewer,
			ILogProvider logProvider,
			Action<HttpConfiguration> startup,
			Type dependencyLoaderType
		) {
			m_serviceDescriptor = serviceDescriptor;
			m_configViewer = configViewer;
			m_logProvider = logProvider;
			m_startup = startup;

			m_dependencyResolver = DependencyResolverFactory
				.CreateFrom( registry =>
					SetupDependencyInjection( dependencyLoaderType, registry )
				);
		}

		private void SetupDependencyInjection(
			Type dependencyLoaderType,
			IDependencyRegistry registry
		) {
			registry.RegisterFactory<ID2LPrincipal, D2LPrincipalFactory>( ObjectScope.WebRequest );

			registry.RegisterInstance<ILogProvider>( m_logProvider );
			registry.RegisterInstance<IConfigViewer>( m_configViewer );

			if( dependencyLoaderType != null ) {
				var method = typeof( IDependencyRegistry )
					.GetMethod( "LoadFrom" )
					.MakeGenericMethod( dependencyLoaderType );

				method.Invoke( registry, null );
			}
		}

		void IDisposable.Dispose() {
			m_disposeHandle.SafeDispose();
		}

		ServiceDescriptor IService.Descriptor {
			get { return m_serviceDescriptor; }
		}

		void IService.Start() {

			ConfigureHttps();

			IService @this = this;

			var options = new StartOptions();

			var host = m_configViewer
				.DangerouslyGetSystemDefaultAsync( Constants.Configs.HOST )
				.SafeWait();

			options.Urls.Add( host.Value );

			m_disposeHandle = WebApp.Start( options, OwinStartup );
		}

		private static void ConfigureHttps() {
			// Work around shitty defaults in .NET 4.5, but be careful to not enable
			// things that might be "bad" in the future by narrowly looking for the
			// .NET 4.5 default settings.

			// This impacts *outgoing* requests from this service (e.g. HttpClient.)

			const SecurityProtocolType NET_4_5_DEFAULT_SETTING =
				SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

			if( ServicePointManager.SecurityProtocol == NET_4_5_DEFAULT_SETTING ) {
				ServicePointManager.SecurityProtocol |=
					SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			}
		}

		public void OwinStartup( IAppBuilder builder ) {
			var config = new HttpConfiguration {
				DependencyResolver = m_dependencyResolver
			};

			var authHandlerFactory = new AuthenticationMessageHandlerFactory();

            // TODO: the auth stuff needs to take IConfigViewer instead so we can override the auth service
			var authEndpoint = m_configViewer
				.DangerouslyGetSystemDefaultAsync( Constants.Configs.AUTH_ENDPOINT )
				.SafeWait();

			var authHandler = authHandlerFactory.Create(
				httpConfiguration: config,
				authenticationEndpoint: new Uri( authEndpoint.Value ),
				logProvider: m_dependencyResolver.Get<ILogProvider>()
			);

			// TODO: use authHandler... need to do this in a way that
			// routes can opt out of it... maybe we pass it into m_startup?

			m_startup( config );

			config.EnsureInitialized();

			builder.UseWebApi( config );
		}
	}
}
