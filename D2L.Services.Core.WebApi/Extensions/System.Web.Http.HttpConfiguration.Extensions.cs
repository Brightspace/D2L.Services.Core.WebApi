using System.Web.Http;
using System.Web.Http.Cors;

namespace D2L.Services {
	public static partial class __D2LServicesCoreWebApi_Extensions {
		public static void EnableCorsForD2LService( this HttpConfiguration @this ) {
			var settings = new EnableCorsAttribute(
				origins: "*",
				headers: "*",
				methods: "*"
			);

			@this.EnableCors( settings );
		}
	}
}
