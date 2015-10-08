using System.Runtime.CompilerServices;
namespace System.Threading.Tasks {
	public static class TaskExtensions {
		public static ConfiguredTaskAwaitable SafeAsync( this Task @this ) {
			return @this.ConfigureAwait( continueOnCapturedContext: false );
		}

		public static ConfiguredTaskAwaitable<T> SafeAsync<T>( this Task<T> @this ) {
			return @this.ConfigureAwait( continueOnCapturedContext: false );
		}

		public static void SafeWait( this Task @this ) {
			@this.SafeAsync().GetAwaiter().GetResult();
		}

		public static T SafeWait<T>( this Task<T> @this ) {
			return @this.SafeAsync().GetAwaiter().GetResult();
		}
	}
}
