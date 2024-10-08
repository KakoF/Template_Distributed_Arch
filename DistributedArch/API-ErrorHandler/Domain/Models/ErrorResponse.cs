using Newtonsoft.Json;

namespace API_ErrorHandler.Domain.Models
{
	public class ErrorResponse
	{
		public int StatusCode { get; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string Message { get; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public object? Errors { get; }
		public ErrorResponse(int statusCode, string? message = null, object? arrayMessage = null)
		{
			StatusCode = statusCode;
			Message = message ?? GetDefaultMessageForStatusCode(statusCode);
			Errors = arrayMessage;
		}
		private static string GetDefaultMessageForStatusCode(int statusCode)
		{
			switch (statusCode)
			{
				case 401:
					return "Unauthorized";
				case 403:
					return "Forbidden";
				case 404:
					return "Not Found";
				case 405:
					return "Not Allowed";
				default:
					return "Internal Server Error";
			}
		}
	}
}