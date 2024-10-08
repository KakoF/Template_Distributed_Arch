namespace API_ErrorHandler.Domain.Models
{
    public class BodyModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = null!;
        public List<string> Errors { get; set; } = null!;
    }
}
