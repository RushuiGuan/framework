namespace Templates.Core {
	public record class CompanyDto {
		public string Name { get; init; }

		public CompanyDto(string name) { 
			this.Name = name;
		}
	}
}