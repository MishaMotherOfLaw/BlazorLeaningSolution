using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Shared.Features.ManageTrails;
using DispatchR.Abstractions.Send;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace BlazingTrails.Api.Features.ManageTrails.Shared;

public class UploadTrailImageEndpoint : EndpointBaseAsync.WithRequest<int>.WithActionResult<string>
{
    private readonly BlazingTrailsContext _database;

    public UploadTrailImageEndpoint(BlazingTrailsContext database)
    {
        _database = database;
    }

    [HttpPost(UploadTrailImageRequest.RouteTemplate)]
    public override async Task<ActionResult<string>> HandleAsync([FromRoute] int trailId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Проверяем существование трейла
            var trail = await _database.Trails.SingleOrDefaultAsync(x => x.Id == trailId, cancellationToken);
            if (trail is null)
            {
                return BadRequest("Trail does not exist.");
            }

            // Проверяем наличие файлов
            if (Request.Form.Files.Count == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var file = Request.Form.Files[0];

            // Проверяем размер файла (например, максимум 10MB)
            if (file.Length == 0)
            {
                return BadRequest("File is empty.");
            }

            if (file.Length > 10 * 1024 * 1024) // 10MB
            {
                return BadRequest("File is too large. Maximum size is 10MB.");
            }

            // Проверяем тип файла
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest("Invalid file type. Only images are allowed.");
            }

            // Создаем папку если не существует
            var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            var filename = $"{Guid.NewGuid()}.jpg";
            var saveLocation = Path.Combine(imagesFolder, filename);

            // Обрабатываем изображение с обработкой ошибок
            try
            {
                var resizeOptions = new ResizeOptions
                {
                    Mode = ResizeMode.Pad,
                    Size = new Size(640, 426)
                };

                using var image = Image.Load(file.OpenReadStream());
                image.Mutate(x => x.Resize(resizeOptions));
                await image.SaveAsJpegAsync(saveLocation, cancellationToken: cancellationToken);
            }
            catch (UnknownImageFormatException)
            {
                return BadRequest("Invalid image format.");
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                Console.WriteLine($"Image processing error: {ex.Message}");
                return BadRequest("Error processing image.");
            }

            // Удаляем старое изображение если есть
            if (!string.IsNullOrWhiteSpace(trail.Image))
            {
                var oldImagePath = Path.Combine(imagesFolder, trail.Image);
                if (System.IO.File.Exists(oldImagePath))
                {
                    try
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                    catch (Exception ex)
                    {
                        // Логируем но не прерываем выполнение
                        Console.WriteLine($"Could not delete old image: {ex.Message}");
                    }
                }
            }

            trail.Image = filename;
            await _database.SaveChangesAsync(cancellationToken);

            return Ok(trail.Image);
        }
        catch (Exception ex)
        {
            // Логируем полную ошибку
            Console.WriteLine($"Unexpected error: {ex}");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}