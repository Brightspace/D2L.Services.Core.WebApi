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
			const string EXPECTED_SERVICE_ID = "f0fe8353-c1b7-42d4-95d4-17b505df668b";

			var bootstrapConfig = new Mock<IConfigViewer>( MockBehavior.Strict );

			bootstrapConfig
				.Setup( cv => cv.TryDangerouslyGetSystemDefaultAsync( Constants.Configs.SERVICE_NAME ) )
				.ReturnsAsync( new ConfigValue(
					name: Constants.Configs.SERVICE_NAME,
					value: EXPECTED_SERVICE_NAME,
					isDefault: true
				 )
			);

			bootstrapConfig
				.Setup( cv => cv.TryDangerouslyGetSystemDefaultAsync( Constants.Configs.SERVICE_ID ) )
				.ReturnsAsync( new ConfigValue(
					name: Constants.Configs.SERVICE_ID,
					value: EXPECTED_SERVICE_ID,
					isDefault: true
				)
			);

			var configViewer = new Mock<IConfigViewer>( MockBehavior.Strict );

			configViewer
				.Setup( cv => cv.TryDangerouslyGetSystemDefaultAsync( Constants.Configs.HOST ) )
				.ReturnsAsync( new ConfigValue(
					name: Constants.Configs.HOST,
					value: "http://+:1234",
					isDefault: true
				)
			); // TODO: pick a port intelligently?

			// TODO: shouldn't be going default styles?
			configViewer
				.Setup( cv => cv.TryDangerouslyGetSystemDefaultAsync( Constants.Configs.AUTH_ENDPOINT ) )
				.ReturnsAsync( new ConfigValue(
					name: Constants.Configs.AUTH_ENDPOINT,
					value: "http://localhost:1235",
					isDefault: true
				)
			);

			using( var service = ServiceFactory.Create<DummyLoader>(
				startup: config => {
				},
				configViewer: configViewer.Object,
				logProvider: NullLogProvider.Instance,
				bootstrapConfigViewer: bootstrapConfig.Object
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
