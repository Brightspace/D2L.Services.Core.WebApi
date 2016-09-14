using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace D2L.Services.Core.WebApi.TestUtils {
	
	internal class TestDependencyScope : IDependencyScope {
		
		protected readonly TestDependencyRegistry m_overrides;
		private readonly IDependencyScope m_innerScope;
		
		internal TestDependencyScope(
			TestDependencyRegistry dependencyOverrides,
			IDependencyScope innerScope
		) {
			m_overrides = dependencyOverrides;
			m_innerScope = innerScope;
		}
		
		object IDependencyScope.GetService( Type serviceType ) {
			object service = null;
			if( m_overrides.TryGet( serviceType, out service ) ) {
				return service;
			}
			
			return m_innerScope.GetService( serviceType );
		}
		
		IEnumerable<object> IDependencyScope.GetServices( Type serviceType ) {
			return m_innerScope.GetServices( serviceType );
		}
		
		void IDisposable.Dispose() {
			m_innerScope.Dispose();
		}
		
	}
	
}
