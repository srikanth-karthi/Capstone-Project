using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

public class BlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    private const string connectionString = "DefaultEndpointsProtocol=http;AccountName=minioadmin;AccountKey=minioadmin;BlobEndpoint=http://localhost:9000/";
    public BlobService(IConfiguration configuration)
    {
        var blobServiceConfig = configuration.GetSection("BlobService");
        var endpoint = blobServiceConfig["Endpoint"];
        var accountName = blobServiceConfig["AccountName"];
        var accountKey = blobServiceConfig["AccountKey"];
        _containerName = blobServiceConfig["ContainerName"];

    _blobServiceClient = new BlobServiceClient(connectionString);
      
    }

    public async Task UploadFileAsync(string filePath, string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(filePath, overwrite: true);
    }

    public async Task<Stream> DownloadFileAsync(string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        var downloadResponse = await blobClient.DownloadAsync();
        return downloadResponse.Value.Content;
    }
}
