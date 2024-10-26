namespace Albatross.CodeGen {
	public interface IConvertObject<From, To> : IConvertObject<From> {
		new To Convert(From from);
	}

	public interface IConvertObject<From> {
		object Convert(From from);
	}
}