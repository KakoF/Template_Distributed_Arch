namespace Domain.Models
{
	public sealed class Cupom : Base
	{
		
		public Guid Id { get; private set; }
		public string Name { get; private set; } = null!;
		public decimal Discount { get; private set; }
		public DateTime? ExpirationDate { get; private set; }


		private Cupom(Guid id, string name, decimal discount, DateTime createdAt, DateTime? expirationDate = null) : base(createdAt)
		{
			Id = id;
			Name = name;
			Discount = discount;
			ExpirationDate = expirationDate;
		}

		public static Cupom Create(string name, decimal discount)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Cupom name cannot be null or empty.", nameof(name));

			if (discount <= 0)
				throw new ArgumentException("Discount must be greater than zero.", nameof(discount));


			return new Cupom(Guid.NewGuid(), name, discount, DateTime.Now);
		}

		public override bool IsValid()
		{
			if (string.IsNullOrWhiteSpace(Name)) return false;
			if (Discount <= 0) return false;
			if (ExpirationDate.HasValue && ExpirationDate.Value < DateTime.UtcNow) return false;

			return true;
		}

		public decimal CalculateDiscount(decimal total)
		{
			if (!IsValid()) return 0m;

			var discountAmount = total * (Discount / 100m);
			return Math.Round(discountAmount, 2);
		}



	}
}
