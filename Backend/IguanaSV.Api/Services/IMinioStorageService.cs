namespace IguanaSV.Api.Services;

public interface IMinioStorageService
{
    Task EnsureBucketExistsAsync(CancellationToken cancellationToken = default);

    Task<string> UploadAsync(IFormFile file, CancellationToken cancellationToken = default);

    Task DeleteAsync(string fileName, CancellationToken cancellationToken = default);
}