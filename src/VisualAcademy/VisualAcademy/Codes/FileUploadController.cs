﻿using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace VisualAcademy.Codes;

[ApiController]
[Route("[controller]")]
public class FileUploadController(
    IWebHostEnvironment environment,
    IConfiguration configuration) : ControllerBase
{
    private readonly string _containerName = "files";

    [HttpGet("uploadfiles")]
    public async Task<IActionResult> UploadFiles()
    {
        var localPath = Path.Combine(environment.WebRootPath, "files");
        await UploadFilesToBlobAsync(localPath);
        return Ok("Files uploaded successfully.");
    }

    [HttpGet("downloadfiles")]
    public async Task<IActionResult> DownloadFiles()
    {
        var localPath = Path.Combine(environment.WebRootPath, "files");
        await DownloadFilesFromBlobAsync(localPath);
        return Ok("Files downloaded successfully.");
    }

    private async Task UploadFilesToBlobAsync(string localPath)
    {
        var connectionString = $"DefaultEndpointsProtocol=https;AccountName=" +
            $"{configuration["AppKeys:AzureStorageAccount"]};AccountKey=" +
            $"{configuration["AppKeys:AzureStorageAccessKey"]};" +
            $"EndpointSuffix=core.windows.net";
        var blobServiceClient = new BlobServiceClient(connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();

        foreach (var filePath in Directory.GetFiles(localPath, "*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(localPath, filePath);
            var blobClient = containerClient.GetBlobClient(relativePath);

            if (await blobClient.ExistsAsync()) // 동일한 파일이 존재하면 건너뜀
                continue;

            using var fileStream = System.IO.File.OpenRead(filePath);
            await blobClient.UploadAsync(fileStream, overwrite: false);
        }
    }

    private async Task DownloadFilesFromBlobAsync(string localPath)
    {
        var connectionString = $"DefaultEndpointsProtocol=https;AccountName=" +
            $"{configuration["AppKeys:AzureStorageAccount"]};AccountKey=" +
            $"{configuration["AppKeys:AzureStorageAccessKey"]};" +
            $"EndpointSuffix=core.windows.net";
        var blobServiceClient = new BlobServiceClient(connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

        await foreach (var blobItem in containerClient.GetBlobsAsync())
        {
            var blobClient = containerClient.GetBlobClient(blobItem.Name);
            var downloadPath = Path.Combine(localPath, blobItem.Name);

            // 해당 디렉터리 생성
            var directoryName = Path.GetDirectoryName(downloadPath);
            if (directoryName == null)
            {
                throw new InvalidOperationException("The directory name cannot be null.");
            }
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            if (System.IO.File.Exists(downloadPath)) // 로컬에 이미 동일한 파일이 존재하면 건너뜀
                continue;

            // 파일 다운로드
            var response = await blobClient.DownloadAsync();
            using (var fileStream = System.IO.File.Create(downloadPath))
            {
                await response.Value.Content.CopyToAsync(fileStream);
            }
        }
    }
}
