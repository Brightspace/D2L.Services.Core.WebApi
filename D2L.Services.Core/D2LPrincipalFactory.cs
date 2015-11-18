using System.Threading;

using D2L.Security.OAuth2.Principal;
using D2L.Services.Core.Activation;

namespace D2L.Services.Core {
	internal sealed class D2LPrincipalFactory : IFactory<ID2LPrincipal> {
		ID2LPrincipal IFactory<ID2LPrincipal>.Create() {
			return Thread.CurrentPrincipal as ID2LPrincipal;
		}
	}
}
