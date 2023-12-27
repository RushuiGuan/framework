namespace Albatross.Hosting {
	public static class Extensions {
		public static void SetCurrentDirectory() {
			System.Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(typeof(Extensions).Assembly.Location)!;
		}
	}
}
