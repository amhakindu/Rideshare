using Rideshare.Application.Contracts.Services;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
namespace Rideshare.Infrastructure.Services;
public class TwilioSmsSender : ISmsSender
{
    private readonly string _accountSid;
    private readonly string _authToken;
    private readonly string _fromPhoneNumber;

    public TwilioSmsSender(string accountSid, string authToken, string fromPhoneNumber)
    {
        _accountSid = accountSid;
        _authToken = authToken;
        _fromPhoneNumber = fromPhoneNumber;
    }

    public async Task SendSmsAsync(string phoneNumber, string message)
    {
        TwilioClient.Init(_accountSid, _authToken);

        await MessageResource.CreateAsync(
            to: new PhoneNumber(phoneNumber),
            from: new PhoneNumber(_fromPhoneNumber),
            body: message);
    }
}