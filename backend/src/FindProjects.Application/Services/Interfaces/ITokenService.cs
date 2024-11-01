using FindProjects.Core.Entities;

namespace FindProjects.Application.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}