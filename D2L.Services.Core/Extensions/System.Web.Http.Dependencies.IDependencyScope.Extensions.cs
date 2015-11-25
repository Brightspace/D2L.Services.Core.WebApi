using System.Web.Http.Dependencies;

namespace D2L {
	public static partial class _D2LServicesCore_Extensions {
		// TODO: move to D2L.Services.Core.Extensions and make public? Downsides: people shouldn't have to use this.
		internal static T Get<T>( this IDependencyScope @this ) {
			return (T)@this.GetService( typeof( T ) );
		}
	}
}
