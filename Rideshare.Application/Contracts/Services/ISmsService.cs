namespace Rideshare.Application.Contracts.Services;
public interface ISmsSender
{
    Task SendSmsAsync(string phoneNumber, string message);
}