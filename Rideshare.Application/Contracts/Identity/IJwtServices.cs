

using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Domain.Models;

namespace Rideshare.Application.Contracts.Identity;

public interface IJwtService
{
    Task<TokenDto> GenerateToken(ApplicationUser user);
    Task<TokenDto?> RefreshToken(TokenDto tokenDto);
}