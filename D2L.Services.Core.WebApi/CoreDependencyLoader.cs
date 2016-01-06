using System;
using D2L.Services.Core.Activation;
using D2L.Services.Core.Configuration;
using SimpleLogInterface;

namespace D2L.Services.Core.WebApi {
	internal sealed class CoreDependencyLoader : IDependencyLoader {
		private readonly IConfigViewer m_configViewer;
		private readonly ILogProvider m_logProvider;
		private readonly Type m_dependencyLoaderType;

		public CoreDependencyLoader(
			IConfigViewer configViewer,
			ILogProvider logProvider,
			Type dependencyLoaderType
		) {
			m_configViewer = configViewer;
			m_logProvider = logProvider;
			m_dependencyLoaderType = dependencyLoaderType;
		}

		void IDependencyLoader.Load( IDependencyRegistry registry ) {
			registry.RegisterInstance<IConfigViewer>( m_configViewer );
			registry.RegisterInstance<ILogProvider>( m_logProvider );

			registry.LoadFrom<Auth.DependencyLoader>();

			// If the user provided a type for their core loader, load from it
			// TODO: this seems kinda lame. Reconsider.
			if( m_dependencyLoaderType != null ) {
				var method = typeof( IDependencyRegistry )
					.GetMethod( "LoadFrom" )
					.MakeGenericMethod( m_dependencyLoaderType );

				method.Invoke( registry, null );
			}
		}
	}
}
