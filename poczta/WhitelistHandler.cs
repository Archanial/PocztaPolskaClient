using System.ServiceModel.Channels;
using SoapCore.Extensibility;

namespace poczta;

public sealed class WhitelistHandler(AllowedAddresses whitelist) : ISoapMessageProcessor
{
    public async Task<Message> ProcessMessage(Message message, HttpContext context, Func<Message, Task<Message>> next)
    {
        if (!IsAddressAllowed(context.Connection.RemoteIpAddress!.ToString()))
            throw new UnauthorizedAccessException("Unauthorized access");
        
        var responseMessage = await next(message);

        return responseMessage;
    }

    private bool IsAddressAllowed(string address)
    {
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        return !whitelist.IsEnabled || (whitelist.Addresses?.Contains(address) ?? false);
    }
}