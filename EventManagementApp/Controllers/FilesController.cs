using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly BlobService _blobService;

    public FilesController(BlobService blobService)
    {
        _blobService = blobService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file.Length > 0)
        {
            var filePath = Path.GetTempFileName();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            await _blobService.UploadFileAsync(filePath, file.FileName);

            System.IO.File.Delete(filePath);
            return Ok(new { FileName = file.FileName });
        }

        return BadRequest("No file uploaded.");
    }

    [HttpGet("download/{blobName}")]
    public async Task<IActionResult> Download(string blobName)
    {
        var stream = await _blobService.DownloadFileAsync(blobName);
        return File(stream, "application/octet-stream", blobName);
    }
}
