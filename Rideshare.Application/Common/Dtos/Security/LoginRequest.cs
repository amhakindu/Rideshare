namespace Rideshare.Application.Common.Dtos.Security;

public sealed record LoginRequest(string UserName, string Password);