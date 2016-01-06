using System.Web.Http;

namespace D2L.Services.Core.WebApi.Auth {
	internal interface IWebApiAuthConfigurator {
		void Configure( HttpConfiguration config );
	}
}
