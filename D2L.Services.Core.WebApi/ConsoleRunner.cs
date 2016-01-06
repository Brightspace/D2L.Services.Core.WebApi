using System;

namespace D2L.Services.Core.WebApi {
	public static class ConsoleRunner {
		public static void Run( IService service ) {
			Console.Title = service.Descriptor.Name;

			service.Start();

			Console.WriteLine( "Running " + service.Descriptor.Name + "..." );
			Console.ReadLine();
		}
	}
}