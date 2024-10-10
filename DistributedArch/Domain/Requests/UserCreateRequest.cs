using System.ComponentModel.DataAnnotations;

namespace Domain.Requests
{
	public record UserCreateRequest(
		[Required]
		[MaxLength(250)]
		string Name
		);
}