using BlazingTrails.Shared.Features.ManageTrails;
using BlazingTrails.Shared.Features.ManageTrails.Requests;
using DispatchR;
using DispatchR.Abstractions.Send;
using System.Net.Http.Json;

namespace BlazingTrails.Client.Features.Home
{
    public class GetTrailsHandler : IRequestHandler<GetTrailsRequest, ValueTask<GetTrailsRequest.Response?>>
    {
        private readonly HttpClient _httpClient;
        public GetTrailsHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async ValueTask<GetTrailsRequest.Response?> Handle(GetTrailsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<GetTrailsRequest.Response>(GetTrailsRequest.RouteTemplate);
            }
            catch (HttpRequestException)
            {
                return default!;
            }
        }

    }

}
