using System.Web.Http;
using D2L.Security.OAuth2.Authentication;
using D2L.Security.OAuth2.Authorization;

namespace D2L.Services.Core.WebApi.Auth {
	internal sealed class WebApiAuthConfigurator : IWebApiAuthConfigurator {
		private readonly OAuth2AuthenticationFilter m_authenticationFilter;

		public WebApiAuthConfigurator(
			OAuth2AuthenticationFilter authenticationFilter
		) {
			m_authenticationFilter = authenticationFilter;
		}

		void IWebApiAuthConfigurator.Configure( HttpConfiguration config ) {
			config.Filters.Add( m_authenticationFilter );

			// This forces [RequireScope(...)] and [AllowFrom(...)] attribute usage
			config.Filters.Add( new DefaultAuthorizationAttribute() );
		}
	}
}
