using CommunityToolkit.Diagnostics;

namespace Domain.Models
{
	public class Item
	{
		public Guid Id { get; private set; }
		public string Name { get; private set; }
		public int Quantity { get; private set; }
		public decimal UnitPrice { get; private set; }

		private Item(Guid id, string name, int quantity, decimal unitPrice)
		{
			Id = id;
			Name = name;
			Quantity = quantity;
			UnitPrice = unitPrice;
		}

		public static Item Create(string name, int quantity, decimal unitPrice)
		{
			Guard.IsNotNullOrEmpty(name);
			Guard.IsGreaterThan(quantity, 0);
			Guard.IsGreaterThan(unitPrice, 0);

			return new Item(Guid.NewGuid(), name, quantity, unitPrice);
		}

		public decimal TotalPrice => Quantity * UnitPrice;

	}
}
