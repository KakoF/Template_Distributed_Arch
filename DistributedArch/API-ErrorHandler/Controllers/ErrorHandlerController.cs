using API_ErrorHandler.Domain.Interfaces.Services;
using API_ErrorHandler.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_ErrorHandler.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ErrorHandlerController : ControllerBase
	{
		private readonly ISomeService _service;

		public ErrorHandlerController(ISomeService service)
		{
			_service = service;
		}

		[HttpGet]
		public string Get()
		{
			return _service.Get();
		}

		[HttpGet]
		[Route("SetMessage/{statusCode}/{message}")]
		public string SetMessage(int statusCode, string message)
		{
			return _service.GetMockErro(statusCode, message);
		}


		[HttpGet]
		[Route("DoDivision/{number}")]
		public double DoDivision(string number)
		{
			return _service.DoDivision(number);
		}

		[HttpPost]
		public string SetFullError([FromBody] BodyModel body)
		{
			return _service.GetMockErro(body.StatusCode, body.Message, body.Errors);
		}
	}
}
