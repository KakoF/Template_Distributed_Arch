namespace Domain.Models
{
	public sealed class Order: Base
	{
		public Guid Id { get; private set; }
		private List<Item> _items = new();
		private Cupom? _cupom;
		
		public IReadOnlyCollection<Item> Items => _items.AsReadOnly();
		public decimal TotalPrice => CalculateTotalPrice();


		private Order(Guid id, DateTime dateTime): base(dateTime)
		{
			Id = id;
		}

		public static Order Create()
		{
			return new Order(Guid.NewGuid(), DateTime.Now);
		}

		public void AddItem(Item item)
		{
			_items.Add(item);
		}

		public void ApplyCupom(Cupom cupom)
		{
			if (cupom == null || !cupom.IsValid())
				throw new ArgumentException("Invalid cupom.");

			_cupom = cupom;
		}


		public decimal CalculateTotalPrice()
		{
			var subtotal = _items.Sum(item => item.TotalPrice);
			var discount = _cupom?.CalculateDiscount(subtotal) ?? 0m;
			return subtotal - discount;

		}

		public override bool IsValid()
		{
			return true;
		}
	}
}
