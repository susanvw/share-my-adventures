using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Tokens.Commands.RevokeToken;

public class RevokeCommand : IRequest<Unit>
{
    public string? Username { get; set; }
}
 
public class RevokeCommandHandler(UserManager<Participant> userManager) : IRequestHandler<RevokeCommand, Unit>
{

    public async Task<Unit> Handle(RevokeCommand request, CancellationToken cancellationToken)
    {
        if (request.Username == null)
        {
            throw new ValidationException("Username could not be found.");
        }

        var user = await userManager.FindByNameAsync(request.Username);
        if (user == null) throw new ValidationException("Username could not be found.");

        user.RefreshToken = null;

        var claims = await userManager.GetClaimsAsync(user);
        await userManager.RemoveClaimsAsync(user, claims);
        await userManager.UpdateAsync(user);

         return Unit.Value;
    }
}
