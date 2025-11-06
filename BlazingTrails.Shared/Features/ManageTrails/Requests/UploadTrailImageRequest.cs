using DispatchR.Abstractions.Send;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingTrails.Shared.Features.ManageTrails
{
    public record UploadTrailImageRequest(int TrailId, IBrowserFile File) : IRequest<UploadTrailImageRequest, ValueTask<UploadTrailImageRequest.Response>>
    {
        public const string RouteTemplate ="/api/trails/{trailId}/images";
        public record Response(string ImageName);
    }
}
