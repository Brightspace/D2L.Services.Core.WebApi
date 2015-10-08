using System.Threading;

using D2L.Activation.WebApi;
using D2L.Security.OAuth2.Principal;

namespace D2L.Services.Core {
	internal sealed class D2LPrincipalFactory : IFactory<ID2LPrincipal> {
		ID2LPrincipal IFactory<ID2LPrincipal>.Create() {
			return Thread.CurrentPrincipal as ID2LPrincipal;
		}
	}
}
