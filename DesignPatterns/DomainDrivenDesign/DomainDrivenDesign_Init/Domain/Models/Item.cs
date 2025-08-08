
namespace Domain.Models
{
	public sealed class Item
	{
		public Guid Id { get; private set; }
		public string Name { get; private set; } = null!;
		public int Quantity { get; private set; }
		public decimal UnitPrice { get; private set; }
		public decimal TotalPrice => Math.Round(Quantity * UnitPrice, 2);


		private Item(string name, int quantity, decimal unitPrice)
		{
			Id = Guid.NewGuid();
			Name = name;
			Quantity = quantity;
			UnitPrice = unitPrice;
		}

		public static Item Create(string name, int quantity, decimal unitPrice)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Item name cannot be null or empty.", nameof(name));

			if (quantity <= 0)
				throw new ArgumentException("Item quantity must be greater than zero.", nameof(quantity));

			if (unitPrice <= 0)
				throw new ArgumentException("Unit price must be greater than zero.", nameof(unitPrice));


			return new Item(name, quantity, unitPrice);
		}
	}
}
