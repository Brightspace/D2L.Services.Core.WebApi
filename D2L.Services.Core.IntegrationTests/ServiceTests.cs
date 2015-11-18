using System;
using D2L.Services.Core.Activation;
using D2L.Services.Core.Configuration;
using Moq;
using NUnit.Framework;
using SimpleLogInterface;

namespace D2L.Services.Core.IntegrationTests {
	[TestFixture]
	public class ServiceTests {

		[Test]
		public void Service_Start_Works() {
			const string EXPECTED_SERVICE_NAME = "FakeIntegrationTestService";
			Guid EXPECTED_SERVICE_ID = Guid.NewGuid();

			var descriptor = new ServiceDescriptor(
				id: EXPECTED_SERVICE_ID,
				name: EXPECTED_SERVICE_NAME
			);

			var configViewer = new Mock<IConfigViewer>( MockBehavior.Strict );

			// TODO: shouldn't be going default styles?
			configViewer
				.Setup( cv => cv.TryDangerouslyGetSystemDefaultAsync( Constants.Configs.AUTH_ENDPOINT ) )
				.ReturnsAsync( new ConfigValue(
					name: Constants.Configs.AUTH_ENDPOINT,
					value: "http://localhost:1235",
					isDefault: true
				)
			);

			using( IService service = new Service(
				descriptor: descriptor,
				configViewer: configViewer.Object,
				logProvider: NullLogProvider.Instance,
				startup: config => {},
				dependencyLoaderType: typeof( DummyLoader )
			) ) {
				service.Start();
				Assert.Pass();
			}
		}

		private class DummyLoader : IDependencyLoader {
			void IDependencyLoader.Load( IDependencyRegistry registry ) {
			}
		}
	}
}
