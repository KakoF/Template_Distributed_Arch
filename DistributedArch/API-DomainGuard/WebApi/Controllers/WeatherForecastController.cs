using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var client = new HttpClient();
			client.BaseAddress = new Uri("https://localhost:7241");

			var response = await client.GetAsync("WeatherForecast");

			return Ok(response.Content);

		}
	}
}
