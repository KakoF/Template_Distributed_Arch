using CommunityToolkit.Diagnostics;

namespace Domain.Models
{
	public class Order
	{
		public Guid Id { get; private set; }
		public DateTime CreatedAt { get; private set; }

		private List<Item> _items = new();
		public IReadOnlyCollection<Item> Items => _items.AsReadOnly();
		

		private Order(Guid id, DateTime createdAt)
		{
			Id = id;
			CreatedAt = createdAt;
		}

		public static Order Create()
		{
			return new Order(Guid.NewGuid(), DateTime.UtcNow);
		}

		public void AddItem(Item item)
		{
			Guard.IsNotNull(item);
			_items.Add(item);
		}

		public decimal TotalAmount => _items.Sum(i => i.TotalPrice);

	}
}
