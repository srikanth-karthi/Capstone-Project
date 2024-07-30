using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EventManagementApp.Interfaces.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

public class BlobService: IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public BlobService(IConfiguration configuration)
    {
        var azureConnectionString = configuration.GetValue<string>("AzureStorage:ConnectionString");
        _containerName = configuration.GetValue<string>("AzureStorage:ContainerName");
        _blobServiceClient = new BlobServiceClient(azureConnectionString);
    }

    public async Task<string> UploadFileAsync(IFormFile file, string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();

        string uniqueBlobName = Guid.NewGuid().ToString();
        BlobClient blobClient = containerClient.GetBlobClient(uniqueBlobName);

        using (var stream = file.OpenReadStream())
        {
            var headers = new BlobHttpHeaders
            {
                ContentDisposition = $"inline; filename={file.FileName}"
            };
            await blobClient.UploadAsync(stream, overwrite: true);
            await blobClient.SetHttpHeadersAsync(headers);
            return blobClient.Uri.ToString();
        }
    }

    public async Task<Stream> DownloadFileAsync(string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        var downloadResponse = await blobClient.DownloadAsync();
        return downloadResponse.Value.Content;
    }
}
