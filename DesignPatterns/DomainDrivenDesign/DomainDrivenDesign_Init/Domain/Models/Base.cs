namespace Domain.Models
{
	public abstract class Base
	{
		public DateTime CreatedAt { get; private set; }

		public Base(DateTime createdAt)
		{
			CreatedAt = createdAt;
		}

		public abstract bool IsValid();
	}
}
