using Microsoft.AspNetCore.Mvc;

namespace Travelings.Controllers
{
    [ApiController]
    [Route("api/v1/upload")]
    public class UploadController : ControllerBase
    {
        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private static readonly string[] AllowedVideoExtensions = { ".mp4", ".webm", ".mov" };
        private const long MaxImageSize = 5 * 1024 * 1024; // 5MB
        private const long MaxVideoSize = 50 * 1024 * 1024; // 50MB

        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Nenhum arquivo enviado." });

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var isImage = AllowedImageExtensions.Contains(ext);
            var isVideo = AllowedVideoExtensions.Contains(ext);

            if (!isImage && !isVideo)
                return BadRequest(new { message = "Formato nao permitido. Use JPG, PNG, GIF, WebP, MP4, WebM ou MOV." });

            var maxSize = isVideo ? MaxVideoSize : MaxImageSize;
            if (file.Length > maxSize)
                return BadRequest(new { message = isVideo ? "Video muito grande. Maximo 50MB." : "Imagem muito grande. Maximo 5MB." });

            var uploadsPath = Path.Combine(_env.ContentRootPath, "uploads");
            Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

            var type = isVideo ? "video" : "image";
            return Ok(new { url, fileName, type });
        }
    }
}
