using System.Security.Claims;
using FindProjects.Application.DTOs;

namespace FindProjects.Application.Services.Interfaces;

public interface IContributorService
{
    Task<ResultDto<int>> CreateContributorAsync(int projectId, string userId, ClaimsPrincipal authenticatedUserClaims);

}