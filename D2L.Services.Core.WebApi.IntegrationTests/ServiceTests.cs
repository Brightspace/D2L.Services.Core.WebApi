using System;
using D2L.Services.Core.Activation;
using D2L.Services.Core.Configuration;
using Moq;
using NUnit.Framework;
using SimpleLogInterface;

namespace D2L.Services.Core.WebApi {
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
				.Setup( cv => cv.GetGlobalAsync<Uri>( Constants.Configs.AUTH_ENDPOINT ) )
				.ReturnsAsync( new ConfigValue<Uri>(
					name: Constants.Configs.AUTH_ENDPOINT,
					value: new Uri( "http://localhost:1235" )
				)
			);

			using( IService service = new Service(
				url: "http://+:1234", // TODO: do this smarter
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
