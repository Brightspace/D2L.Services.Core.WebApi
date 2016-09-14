using System;
using System.Web.Http.Dependencies;

namespace D2L.Services.Core.WebApi.TestUtils {
	
	internal sealed class TestDependencyResolver : TestDependencyScope, IDependencyResolver {
		
		private readonly IDependencyResolver m_innerResolver;
		
		internal TestDependencyResolver(
			TestDependencyRegistry dependencyOverrides,
			IDependencyResolver innerResolver
		) : base( dependencyOverrides, innerResolver ) {
			m_innerResolver = innerResolver;
		}
		
		IDependencyScope IDependencyResolver.BeginScope() {
			return new TestDependencyScope(
				m_overrides,
				m_innerResolver.BeginScope()
			);
		}
		
	}
	
}
