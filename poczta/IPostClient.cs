using poczta.Sledzenie;

namespace poczta;

public interface IPostClient
{
    Task<witajResponse> GetWelcomeMessage(string name);

    Task<wersjaResponse> GetVersion();

    Task<sprawdzPrzesylkiPlResponse> CheckLocalShipments(string[] numbers);

    Task<sprawdzPrzesylkiResponse> CheckShipments(string[] numbers);

    Task<sprawdzPrzesylkeResponse> CheckSingleShipment(string number);

    Task<sprawdzPrzesylkePlResponse> CheckSingleLocalShipment(string number);

    Task<sprawdzPrzesylkiOdDoResponse> CheckShipmentsByDate(string[] numbers, string startDate, string endDate);

    Task<sprawdzPrzesylkiOdDoPlResponse> CheckLocalShipmentsByDate(string[] numbers, string startDate, string endDate);

    Task<maksymalnaLiczbaPrzesylekResponse> GetMaxShipments();
}