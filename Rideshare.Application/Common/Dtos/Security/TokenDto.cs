namespace Rideshare.Application.Common.Dtos.Security;

public record TokenDto(string AccessToken, string RefreshToken);