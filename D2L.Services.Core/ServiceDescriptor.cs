using System;

namespace D2L.Services.Core {
	public sealed class ServiceDescriptor {
		private readonly Guid m_id;
		private readonly string m_name;

		public ServiceDescriptor( Guid id, string name ) {
			m_id = id;
			m_name = name;
		}

		/// <summary>
		/// Computer-friendly GUID for this service.
		/// </summary>
		public Guid Id {
			get { return m_id; }
		}

		/// <summary>
		/// Human-friendly name of the service.
		/// </summary>
		public string Name {
			get { return m_name; }
		}
	}
}
