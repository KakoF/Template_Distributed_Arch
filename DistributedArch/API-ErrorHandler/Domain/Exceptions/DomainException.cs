namespace API_ErrorHandler.Domain.Exceptions
{
	public class DomainException : Exception
	{
		internal List<string> _errors = null!;
		internal int _statusCode;

		public IReadOnlyCollection<string> Errors => _errors;

		public int StatusCode => _statusCode;

		public DomainException() { }

		public DomainException(string message, int statusCode = 400) : base(message)
		{
			_statusCode = statusCode;
		}

		public DomainException(string message, List<string> errors, int statusCode = 400) : base(message)
		{
			_statusCode = statusCode;
			_errors = errors;
		}

		public DomainException(string message, Exception innerException, int statusCode = 400) : base(message, innerException)
		{
			_statusCode = statusCode;
		}
	}
}