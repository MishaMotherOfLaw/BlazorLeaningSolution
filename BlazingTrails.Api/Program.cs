using BlazingTrails.Api.Persistence;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder();
        // Добавляем сервисы в контейнер;
        builder.Services.AddDbContext<BlazingTrailsContext>(options =>options.UseSqlite(builder.Configuration.GetConnectionString("BlazingTrailsContext")));
        builder.Services.AddControllers();

        var app = builder.Build();

        // Настройка конвейера HTTP-запросов;
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }

        app.UseHttpsRedirection();

        app.UseBlazorFrameworkFiles();

        app.UseStaticFiles();
        app.UseRouting();
        app.MapControllers();
        app.MapFallbackToFile("index.html");
        app.Run();
    }
}