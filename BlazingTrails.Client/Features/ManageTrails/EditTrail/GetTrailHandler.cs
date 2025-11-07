
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
            var url = GetTrailRequest.RouteTemplate.Replace("{trailId}", request.TrailId.ToString());
            return await _httpClient.GetFromJsonAsync<GetTrailRequest.Response>(url, cancellationToken);
        }
        catch (HttpRequestException)
        {
            return default!;
        }
    }
}