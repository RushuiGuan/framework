namespace Albatross.WebClient {
	public class ArrayDto<T> {
		public string? Schema { get; set; }
		public T[] Data { get; set; } = new T[0];
	}
}
