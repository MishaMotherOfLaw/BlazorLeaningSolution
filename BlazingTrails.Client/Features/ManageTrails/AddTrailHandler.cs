using BlazingTrails.Shared.Features.ManageTrails;
using DispatchR;
using DispatchR.Abstractions.Send;
using System.Net.Http.Json;

namespace BlazingTrails.Client;

public class AddTrailHandler : IRequestHandler<AddTrailRequest, ValueTask<AddTrailRequest.Response>>
{
    private readonly HttpClient _httpClient;

    public AddTrailHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async ValueTask<AddTrailRequest.Response> Handle(AddTrailRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync(AddTrailRequest.RouteTemplate, request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var trailId = await response.Content.ReadFromJsonAsync<int>(cancellationToken: cancellationToken);
            return new AddTrailRequest.Response(trailId);
        }
        else
        {
            return new AddTrailRequest.Response(-1);
        }
    }
}
