namespace EventManagementApp.Interfaces.Service
{
    public interface IBlobService
    {
        Task<string> UploadFileAsync(IFormFile file, string blobName);
        Task<Stream> DownloadFileAsync(string blobName);
    }
}
