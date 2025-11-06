using BlazingTrails.Client;
using BlazingTrails.Client.Features.ManageTrails;
using BlazingTrails.Shared.Features.ManageTrails;
using DispatchR.Abstractions.Send;
using DispatchR.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadContent>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


builder.Services.AddDispatchR(typeof(Program).Assembly);

await builder.Build().RunAsync();
