using System;
using Owin;

namespace D2L.Services.Core {
	public interface IService : IDisposable {
		ServiceDescriptor Descriptor { get; }
		void Start();
		void OwinStartup( IAppBuilder appBuilder );
	}
}
