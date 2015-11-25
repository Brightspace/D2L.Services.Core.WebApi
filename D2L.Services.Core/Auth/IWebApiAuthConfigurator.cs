using System.Web.Http;

namespace D2L.Services.Core.Auth {
	internal interface IWebApiAuthConfigurator {
		void Configure( HttpConfiguration config );
	}
}
