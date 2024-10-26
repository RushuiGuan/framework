namespace Test.Dto.Classes {
	public class GenericBaseType<T> {
		public T Value { get; set; }
		public string Name { get; set; }
		public GenericBaseType(string name, T value) {
			Name = name;
			Value = value;
		}
	}
}