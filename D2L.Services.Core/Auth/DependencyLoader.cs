using D2L.Security.OAuth2.Authentication;
using D2L.Security.OAuth2.Principal;
using D2L.Security.OAuth2.Validation.Request;
using D2L.Services.Core.Activation;

namespace D2L.Services.Core.Auth {
	internal sealed class DependencyLoader : IDependencyLoader {
		void IDependencyLoader.Load( IDependencyRegistry registry ) {
			registry.Register<ID2LPrincipal, D2LPrincipalWrapper>( ObjectScope.WebRequest );
			registry.Register<ID2LPrincipalDependencyRegistry, D2LPrincipalDependencyRegistry>( ObjectScope.Singleton );
			registry.RegisterFactory<IRequestAuthenticator, Temp_RequestAuthenticatorFactory>( ObjectScope.Singleton );
			registry.Register<IWebApiAuthConfigurator, WebApiAuthConfigurator>( ObjectScope.Singleton );
			registry.Register<OAuth2AuthenticationFilter>( ObjectScope.Singleton );
		}
	}
}
