using System.ServiceModel;
using poczta.Sledzenie;

namespace poczta;

[ServiceContract]
public interface IPostApi
{
    [OperationContract]
    Task<string> GetWelcomeMessage(string name);
    
    [OperationContract]
    Task<string> GetVersion();

    [OperationContract]
    Task<Komunikat> CheckLocalShipments(string[] numbers);

    [OperationContract]
    Task<Komunikat> CheckShipments(string[] numbers);

    [OperationContract]
    Task<Przesylka> CheckSingleShipment(string number);

    [OperationContract]
    Task<Przesylka> CheckSingleLocalShipment(string number);

    [OperationContract]
    Task<Komunikat> CheckShipmentsByDate(string[] numbers, DateTime startDate, DateTime endDate);

    [OperationContract]
    Task<Komunikat> CheckLocalShipmentsByDate(string[] numbers, DateTime startDate, DateTime endDate);

    [OperationContract]
    Task<int> GetMaxShipments();
}