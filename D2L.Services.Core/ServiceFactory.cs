using System;
using System.Threading.Tasks;
using System.Web.Http;
using D2L.Services.Core.Configuration;
using D2L.Services.Core.Configuration.Viewers;
using SimpleLogInterface;

namespace D2L.Services.Core {
	public static class ServiceFactory {
		public static IService Create<CoreDependencyLoader>(
			Action<HttpConfiguration> startup,
			IConfigViewer configViewer,
			ILogProvider logProvider,
			IConfigViewer bootstrapConfigViewer = null
		) {

			if (bootstrapConfigViewer == null) {
				bootstrapConfigViewer = new AppConfigConfigViewer();
			}

			var name = bootstrapConfigViewer
				.GetInstanceAsync( Constants.Configs.SERVICE_NAME )
				.SafeWait();

			var serviceId = Guid.Parse(
				bootstrapConfigViewer
					.GetInstanceAsync( Constants.Configs.SERVICE_ID )
					.SafeWait()
			);

			var descriptor = new ServiceDescriptor( serviceId, name );

			return new Service(
				descriptor,
				configViewer,
				logProvider,
				startup,
				typeof( CoreDependencyLoader )
			);
		}
	}
}
