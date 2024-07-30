using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        try
        {
            var result = await _blobService.UploadFileAsync(file, file.FileName);
            return Ok(new { Uri = result });
        }
        catch (Exception ex)
        {
            // Log the exception here (ex)
            return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading file.");
        }
    }
}
