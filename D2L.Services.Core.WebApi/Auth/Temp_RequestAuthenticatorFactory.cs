using System;
using System.Net.Http;
using D2L.Security.OAuth2.Validation.AccessTokens;
using D2L.Security.OAuth2.Validation.Request;
using D2L.Services.Core.Activation;
using D2L.Services.Core.Configuration;

namespace D2L.Services.Core.WebApi.Auth {
	/// <summary>
	/// This class is "temp" because we shouldn't need a factory once we refactor RequestAuthenticator and friends
	/// </summary>
	internal sealed class Temp_RequestAuthenticatorFactory : IFactory<IRequestAuthenticator> {
		private readonly IConfigViewer m_configViewer;

		public Temp_RequestAuthenticatorFactory(
			IConfigViewer configViewer
		) {
			m_configViewer = configViewer;	
		}

		IRequestAuthenticator IFactory<IRequestAuthenticator>.Create() {
			// TODO:
			// - This blocking sucks but will disappear once IConfigViewer is taken rather than the actual endpoint
			// - We need these classes from the auth library to behave like singletons but get the auth endpoint dynamically from IConfigViewer
			// - This config should usually be fetched as a tenant config... when we support non-tenant access tokens we can switch config type inside the auth library
			ConfigValue<Uri> authEndpoint = m_configViewer
				.GetGlobalAsync<Uri>( Constants.Configs.AUTH_ENDPOINT )
				.SafeWait();

			var accessTokenValidator = AccessTokenValidatorFactory.CreateRemoteValidator(
				new HttpClient(), // TODO: get rid of AccessTokenValidatorFactory (maybe), make AccessTokenValidatorFactory take an IHttpClientFactory
				authEndpoint.Value // TODO: AccessTokenValidator should take an IConfigViewer
			);

			var requestAuthenticator = RequestAuthenticatorFactory.Create(
				accessTokenValidator: accessTokenValidator
			);

			return requestAuthenticator;
		}
	}
}
