using System.ServiceModel;
using poczta.Sledzenie;

namespace poczta;

public sealed class PostClient(SledzeniePortTypeClient client) : IPostClient
{
    private static readonly SecurityHeader SecurityHeader = new("sledzeniepp", "PPSA");
    
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
        using var _ = new OperationContextScope(client.InnerChannel);
        var messageHeadersElement = OperationContext.Current.OutgoingMessageHeaders;
        messageHeadersElement.Add(SecurityHeader);
        return action();
    }
}