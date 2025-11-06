using BlazingTrails.Shared.Features.ManageTrails.Requests;
using DispatchR.Abstractions.Send;
using System.Net.Http.Json;

namespace BlazingTrails.Client.Features.ManageTrails.EditTrail;

public class EditTrailHandler : IRequestHandler<EditTrailRequest, ValueTask<EditTrailRequest.Response>>
{
    private readonly HttpClient _httpClient;

    public EditTrailHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async ValueTask<EditTrailRequest.Response> Handle(EditTrailRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PutAsJsonAsync(EditTrailRequest.RouteTemplate, request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return new EditTrailRequest.Response(true);
        }
        else
        {
            return new EditTrailRequest.Response(false);
        }
    }
}