using DispatchR.Abstractions.Send;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingTrails.Shared.Features.ManageTrails.Requests
{
    public record EditTrailRequest(TrailDto Trail) :IRequest<EditTrailRequest,ValueTask<EditTrailRequest.Response>>
    {
        public const string RouteTemplate = "/api/trails";
        public record Response(bool IsSuccess);
    }
    public class EditTrailRequestValidator : AbstractValidator<EditTrailRequest>
    {
        public EditTrailRequestValidator()
        {
            RuleFor(x => x.Trail).SetValidator(new TrailValidator());
        }
    }
}
