using BlazingTrails.Shared.Features.ManageTrails.Shared;
using DispatchR;
using DispatchR.Abstractions.Send;
using FluentValidation;

namespace BlazingTrails.Shared.Features.ManageTrails.Requests;

public record AddTrailRequest(TrailDto Trail) : IRequest<AddTrailRequest, ValueTask<AddTrailRequest.Response>>
{
    public const string RouteTemplate = "/api/trails";

    public record Response(int TrailId);
}

public class AddTrailRequestValidator : AbstractValidator<AddTrailRequest>
{
    public AddTrailRequestValidator()
    {
        RuleFor(x => x.Trail).SetValidator(new TrailValidator());
    }
}
