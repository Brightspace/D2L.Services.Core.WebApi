using System;
using System.Web.Http.Dependencies;
using D2L.Services.Core.Activation;
using D2L.Services.Core.WebApi.TestUtils;
using NUnit.Framework;

namespace D2L.Services.Core.WebApi {
	
	[TestFixture]
	public sealed class TestDependencyResolverTests {
		
		private interface TestInterface {}
		private sealed class OriginalType : TestInterface {}
		private sealed class OverrideType : TestInterface {}
		
		private IDependencyResolver m_resolver;
		private TestDependencyRegistry m_testDependencyRegistry;
		
		[TestFixtureSetUp]
		public void TestFixtureSetUp() {
			m_testDependencyRegistry = new TestDependencyRegistry();
			m_resolver = new TestDependencyResolver(
				m_testDependencyRegistry,
				DependencyResolverFactory.CreateFrom<TestDependencyLoader>()
			);
		}
		
		[TestFixtureTearDown]
		public void TestFixtureTearDown() {
			m_resolver.SafeDispose();
		}
		
		[SetUp]
		public void SetUp() {
			m_testDependencyRegistry.ClearAllOverrides();
		}
		
		[Test]
		public void SimpleTest() {
			Assert.IsTrue( m_resolver.Get<TestInterface>() is OriginalType );
			m_testDependencyRegistry.OverrideDependency<TestInterface>( new OverrideType() );
			Assert.IsTrue( m_resolver.Get<TestInterface>() is OverrideType );
			m_testDependencyRegistry.ClearOverride<TestInterface>();
			Assert.IsTrue( m_resolver.Get<TestInterface>() is OriginalType );
		}
		
		[Test]
		public void DependencyScopeTest() {
			using( IDependencyScope scope = m_resolver.BeginScope() ) {
				Assert.IsTrue( m_resolver.Get<TestInterface>() is OriginalType );
				Assert.IsTrue( scope.Get<TestInterface>() is OriginalType );
				
				m_testDependencyRegistry.OverrideDependency<TestInterface>( new OverrideType() );
				
				Assert.IsTrue( m_resolver.Get<TestInterface>() is OverrideType );
				Assert.IsTrue( scope.Get<TestInterface>() is OverrideType );
				
				m_testDependencyRegistry.ClearOverride<TestInterface>();
				
				Assert.IsTrue( m_resolver.Get<TestInterface>() is OriginalType );
				Assert.IsTrue( scope.Get<TestInterface>() is OriginalType );
			}
		}
		
		
		private sealed class TestDependencyLoader : IDependencyLoader {
			void IDependencyLoader.Load( IDependencyRegistry registry ) {
				registry.Register<TestInterface,OriginalType>( ObjectScope.Singleton );
			}
		}
		
	}
	
}
