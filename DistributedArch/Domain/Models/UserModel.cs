namespace Domain.Models
{
	public class UserModel
	{
		public long Id { get; private set; }
		public string Name { get; private set; } = null!;

		public UserModel(long id, string name)
		{
			Id = id;
			Name = name;
		}
		public UserModel(string name)
		{
			Name = name;
		}
		public void Update(string name)
		{
			Name = name;
		}
	}
}
