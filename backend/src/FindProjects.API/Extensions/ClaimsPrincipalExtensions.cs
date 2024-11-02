using System.Security.Claims;

namespace FindProjects.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new InvalidOperationException("Não foi possível obter o Id do usuário");
        }

        return userId;
    }
}