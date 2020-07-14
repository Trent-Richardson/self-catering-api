using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServerHost.Quickstart.UI;

public class DemoProfileService : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        await Task.CompletedTask;
        var user = TestUsers.Users.FirstOrDefault(x => x.SubjectId == context.Subject.Claims.FirstOrDefault(y => y.Type == JwtClaimTypes.Subject).Value);
        if (user != null)
        {
            var emailVerifiedClaim = user.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.EmailVerified);
            if (emailVerifiedClaim != null)
                context.IssuedClaims.Add(emailVerifiedClaim);
        }
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        await Task.CompletedTask;
        var user = TestUsers.Users.FirstOrDefault(x => x.SubjectId == context.Subject.Claims.FirstOrDefault(y => y.Type == JwtClaimTypes.Subject).Value);
        var isActive = (user != null) && user.IsActive;
        context.IsActive = isActive;
    }
}