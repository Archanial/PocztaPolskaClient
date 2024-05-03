using System.ServiceModel;
using Microsoft.Extensions.Options;
using poczta.Sledzenie;

namespace poczta;

public sealed class PostClient(SledzeniePortTypeClient client, IOptions<ApiCredentials> credentials) : IPostClient
{
    private static SecurityHeader? _securityHeader;
    
    public Task<witajResponse> GetWelcomeMessage(string name) 
        => ExecuteWithSecurityHeader(() => client.witajAsync(name));

    public Task<wersjaResponse> GetVersion() => ExecuteWithSecurityHeader(client.wersjaAsync);
    
    public Task<sprawdzPrzesylkiPlResponse> CheckLocalShipments(string[] numbers)
        => ExecuteWithSecurityHeader(() => client.sprawdzPrzesylkiPlAsync(numbers));
    
    public Task<sprawdzPrzesylkiResponse> CheckShipments(string[] numbers)
        => ExecuteWithSecurityHeader(() => client.sprawdzPrzesylkiAsync(numbers));
    
    public Task<sprawdzPrzesylkeResponse> CheckSingleShipment(string number)
        => ExecuteWithSecurityHeader(() => client.sprawdzPrzesylkeAsync(number));
    
    public Task<sprawdzPrzesylkePlResponse> CheckSingleLocalShipment(string number)
        => ExecuteWithSecurityHeader(() => client.sprawdzPrzesylkePlAsync(number));

    public Task<sprawdzPrzesylkiOdDoResponse> CheckShipmentsByDate(string[] numbers, string startDate, string endDate)
        => ExecuteWithSecurityHeader(() => client.sprawdzPrzesylkiOdDoAsync(numbers, startDate, endDate));
    
    public Task<sprawdzPrzesylkiOdDoPlResponse> CheckLocalShipmentsByDate(string[] numbers,
        string startDate,
        string endDate)
        => ExecuteWithSecurityHeader(() => client.sprawdzPrzesylkiOdDoPlAsync(numbers, startDate, endDate));
    
    public Task<maksymalnaLiczbaPrzesylekResponse> GetMaxShipments()
        => ExecuteWithSecurityHeader(client.maksymalnaLiczbaPrzesylekAsync);

    private Task<T> ExecuteWithSecurityHeader<T>(Func<Task<T>> action)
    {
        _securityHeader ??= new SecurityHeader(credentials.Value.Username, credentials.Value.Password);
        
        using var _ = new OperationContextScope(client.InnerChannel);
        var messageHeadersElement = OperationContext.Current.OutgoingMessageHeaders;
        messageHeadersElement.Add(_securityHeader);
        return action();
    }
}