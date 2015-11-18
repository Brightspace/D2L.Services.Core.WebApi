using System;
namespace D2L.Services.Core {
	public sealed class ServiceDescriptor {
		private readonly Guid m_serviceId;
		private readonly string m_name;

		internal ServiceDescriptor(
			Guid serviceId,
			string name
		) {
			m_serviceId = serviceId;
			m_name = name;
		}

		public Guid ServiceId {
			get { return m_serviceId; }
		}

		public string Name {
			get { return m_name; }
		}
	}
}
