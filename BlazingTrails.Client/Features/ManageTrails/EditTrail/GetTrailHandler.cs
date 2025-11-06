
using BlazingTrails.Shared.Features.ManageTrails.Requests;
using DispatchR.Abstractions.Send;
using System.Net.Http.Json;

namespace BlazingTrails.Client.Features.ManageTrails.EditTrail;

public class GetTrailHandler : IRequestHandler<GetTrailRequest, ValueTask<GetTrailRequest.Response?>>
{
    private readonly HttpClient _httpClient;

    public GetTrailHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async ValueTask<GetTrailRequest.Response?> Handle(GetTrailRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<GetTrailRequest.Response>(GetTrailRequest.RouteTemplate.Replace("{trailId}", request.TrailId.ToString()));
        }
        catch (HttpRequestException)
        {
            return default!;
        }
    }
}