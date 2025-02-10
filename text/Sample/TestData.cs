namespace Sample {
	public record class Contact {
		public string Name { get; set; } = string.Empty;
		public int Age { get; set; }
		public string? Address { get; set; }
		public string? City { get; set; }
		public string? State { get; set; }
		public string? Zip { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
	}
}
