using System;
using System.Collections.Generic;
using D2L.Security.OAuth2.Principal;
using D2L.Security.OAuth2.Scopes;
using D2L.Security.OAuth2.Validation.AccessTokens;
using Microsoft.Practices.Unity;

namespace D2L.Services.Core.Auth {
	internal sealed class D2LPrincipalWrapper : ID2LPrincipal {
		// Excessive, but will make it less likely to collide with anything/anyone
		// getting funny ideas about grabbing this from outside the assembly (not
		// that we can actually stop them.)
		internal static readonly string DI_INNER_NAME = Guid.NewGuid().ToString() + "_innerPrincipal";

		private readonly Lazy<ID2LPrincipal> m_principal;

		public D2LPrincipalWrapper(
			IUnityContainer container
		) {
			m_principal = new Lazy<ID2LPrincipal>( () => GetPrincipalFromDI( container ) );			
		}

		IAccessToken ID2LPrincipal.AccessToken {
			get { return m_principal.Value.AccessToken; }
		}

		IEnumerable<Scope> ID2LPrincipal.Scopes {
			get { return m_principal.Value.Scopes; }
		}

		Guid ID2LPrincipal.TenantId {
			get { return m_principal.Value.TenantId; }
		}

		PrincipalType ID2LPrincipal.Type {
			get { return m_principal.Value.Type; }
		}

		string ID2LPrincipal.UserId {
			get { return m_principal.Value.UserId; }
		}

		private static ID2LPrincipal GetPrincipalFromDI( IUnityContainer container ) {
			ID2LPrincipal principal;

			try {
				principal = (ID2LPrincipal)container
					.Resolve(
						name: DI_INNER_NAME,
						t: typeof( ID2LPrincipal )
					);
			} catch( ResolutionFailedException rfe ) {
				throw new Exception(
						@"Could not resolve ID2LPrincipal.  This is probably because you accessed the properties of the Principal from the constructor of a Controller.  See D2LPrincipalWrapper for more explanation.",
						rfe
					);
			}

			return principal;
		}
	}
}
