using CommunityToolkit.Diagnostics;
using System.Xml.Linq;

namespace Domain.Models
{
	public sealed class User
	{
		public Guid Id { get; private set; }
		public string Name { get; private set; } = null!;

		private List<Order> _orders = new();
		public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

		private User(Guid id, string name)
		{
			Id = id;
			Name = name;
		}

		public static User Create(string name)
		{
			Guard.IsNotNullOrEmpty(name);
			return new User(Guid.NewGuid(), name);
		}

		public static User Clone(Guid id, string name)
		{
			return new User(id, name);
		}

		public void PlaceOrder(Order order)
		{
			Guard.IsNotNull(order);
			_orders.Add(order);
		}

	}
}
