using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using D2L.Security.OAuth2.Authentication;
using D2L.Security.OAuth2.Authorization;
using D2L.Security.OAuth2.Principal;
using D2L.Security.OAuth2.Validation.AccessTokens;
using D2L.Security.OAuth2.Validation.Request;
using D2L.Services.Core.Configuration;
using Microsoft.Practices.Unity;
using SimpleLogInterface;

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
