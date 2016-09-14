
using System;
using System.Collections.Generic;

namespace D2L.Services.Core.WebApi.TestUtils {
	
	public sealed class TestDependencyRegistry {
		
		private readonly IDictionary<Type,object> m_overrides;
		
		public TestDependencyRegistry() {
			m_overrides = new Dictionary<Type,object>();
		}
		
		public void OverrideDependency<DependencyType>(
			DependencyType dependencyOverride
		) {
			m_overrides[typeof( DependencyType )] = (object)dependencyOverride;
		}
		
		public void ClearOverride<DependencyType>() {
			m_overrides.Remove( typeof( DependencyType ) );
		}
		
		public void ClearAllOverrides() {
			m_overrides.Clear();
		}
		
		internal bool TryGet(
			Type dependencyType,
			out object dependencyOverride
		) {
			return m_overrides.TryGetValue( dependencyType, out dependencyOverride );
		}
		
	}
	
}
