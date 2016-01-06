using System.Web.Http.Filters;
using D2L.Security.OAuth2.Authentication;
using D2L.Security.OAuth2.Principal;
using Microsoft.Practices.Unity;

namespace D2L.Services.Core.WebApi.Auth {
	internal sealed class D2LPrincipalDependencyRegistry : ID2LPrincipalDependencyRegistry {
		void ID2LPrincipalDependencyRegistry.Register(
			HttpAuthenticationContext context,
			ID2LPrincipal principal
		) {
			// TODO: in D2L.Services.Core.Activation we should make IDependencyRegistry
			// injectible as an alternative to IUnityContainer (and block injection of that
			// if possible) so that we aren't coupling this code to unity.
			var container = context.Request.GetDependency<IUnityContainer>();

			container.RegisterInstance(
				name: D2LPrincipalWrapper.DI_INNER_NAME,
				instance: principal
			);
		}
	}
}
