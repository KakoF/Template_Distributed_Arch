using Microsoft.AspNetCore.Mvc;

namespace FileStreamingApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileStreamController : ControllerBase
    {
        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile()
        {
            var filePath = @"E:\File.rar"; // Substitua pelo caminho do seu arquivo

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var response = File(fileStream, "application/octet-stream", Path.GetFileName(filePath));
            response.EnableRangeProcessing = true;
            
            return response;
        }

		[HttpGet("download/video")]
		public async Task<IActionResult> DownloadVideo()
		{
			var filePath = @"E:\Video.mp4"; // Substitua pelo caminho do seu arquivo

			if (!System.IO.File.Exists(filePath))
				return NotFound();

			var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			var response = File(fileStream, "video/mp4", Path.GetFileName(filePath));
			response.EnableRangeProcessing = true;

			return response;
		}

	}
}
