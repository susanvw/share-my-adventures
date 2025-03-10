using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Profiles.Queries;

public sealed record GetProfileByUserIdQuery : IRequest<Result<ProfileView?>>
{
    public required string UserId { get; init; } = string.Empty;
} 

public sealed class GetProfileByUserIdQueryHandler(
    UserManager<Participant> userManager
    ) : IRequestHandler<GetProfileByUserIdQuery, Result<ProfileView?>>
{

    public async Task<Result<ProfileView?>> Handle(GetProfileByUserIdQuery request, CancellationToken cancellationToken)
    {
        var participant = await userManager.FindByIdAsync(request.UserId);
        participant = participant.ThrowIfNotFound(request.UserId);

        return Result<ProfileView?>.Success(ProfileView.MapFrom(participant));
    }
}
