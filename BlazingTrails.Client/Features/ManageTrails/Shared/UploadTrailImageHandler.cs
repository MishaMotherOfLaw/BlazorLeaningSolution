using BlazingTrails.Shared.Features.ManageTrails;
using DispatchR.Abstractions.Send;
namespace BlazingTrails.Client.Features.ManageTrails.Shared;

public class UploadTrailImageHandler : IRequestHandler<UploadTrailImageRequest, ValueTask<UploadTrailImageRequest.Response>>
{
    private readonly HttpClient _httpClient;

    public UploadTrailImageHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async  ValueTask<UploadTrailImageRequest.Response> Handle(UploadTrailImageRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Открываем поток внутри using
            using var fileStream = request.File.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024, cancellationToken);
            using var streamContent = new StreamContent(fileStream);

            using var formData = new MultipartFormDataContent();
            formData.Add(streamContent, "image", request.File.Name);

            var response = await _httpClient.PostAsync(
                UploadTrailImageRequest.RouteTemplate.Replace("{trailId}", request.TrailId.ToString()),
                formData,
                cancellationToken
            );

            if (response.IsSuccessStatusCode)
            {
                var fileName = await response.Content.ReadAsStringAsync(cancellationToken);
                return new UploadTrailImageRequest.Response(fileName.Trim('"')); // Убираем кавычки если есть
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                Console.WriteLine($"Upload failed: {response.StatusCode} - {error}");
                return new UploadTrailImageRequest.Response("");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Upload error: {ex.Message}");
            return new UploadTrailImageRequest.Response("");
        }
    }
}