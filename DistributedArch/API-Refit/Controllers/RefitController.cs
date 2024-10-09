using API_Refit.Infrastruct.Interfaces;
using API_Refit.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_Refit.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class RefitController : ControllerBase
	{
		private readonly IRefitService _service;

		public RefitController(IRefitService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<string> GetAsync()
		{
			return await _service.GetAsync();
		}

		[HttpGet]
		[Route("DoDivision/{number}")]
		public async Task<double> DoDivisionAsync(string number)
		{
			return await _service.DoDivisionAsync(number);
		}
	}
}
