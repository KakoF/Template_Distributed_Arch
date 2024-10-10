using System.ComponentModel.DataAnnotations;

namespace API_EntityFramework.Domain.Requests
{
	public record UserCreateRequest(
		[Required]
		[MaxLength(250)]
		string Name
		);
}
