using D2L.Activation.WebApi;
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
				.Setup( cv => cv.TryGetInstanceAsync( Constants.Configs.SERVICE_NAME ) )
				.ReturnsAsync( EXPECTED_SERVICE_NAME );

			bootstrapConfig
				.Setup( cv => cv.TryGetInstanceAsync( Constants.Configs.SERVICE_ID ) )
				.ReturnsAsync( EXPECTED_SERVICE_ID );

			var configViewer = new Mock<IConfigViewer>( MockBehavior.Strict );

			configViewer
				.Setup( cv => cv.TryGetInstanceAsync( Constants.Configs.HOST ) )
				.ReturnsAsync( "http://+:1234" ); // TODO: pick a port intelligently?

			configViewer
				.Setup( cv => cv.TryGetInstanceAsync( Constants.Configs.AUTH_ENDPOINT ) )
				.ReturnsAsync( "http://localhost:1235" );

			using (var service = ServiceFactory.Create<DummyLoader>(
				startup: config => {},
				configViewer: configViewer.Object,
				logProvider: NullLogProvider.Instance,
				bootstrapConfigViewer: bootstrapConfig.Object
			)) {
				service.Start();
				Assert.Pass();
			}
		}

		private class DummyLoader : IDependencyLoader {
			void IDependencyLoader.Load(IDependencyRegistry registry) {}
		}
    }
}
