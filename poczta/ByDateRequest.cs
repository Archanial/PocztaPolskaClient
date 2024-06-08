namespace poczta;

public sealed class ByDateRequest
{
    public string[]? Numbers { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
}