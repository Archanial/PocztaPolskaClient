namespace poczta;

public sealed class AllowedAddresses
{
    public required bool IsEnabled { get; set; }
    
    public required string[] Addresses { get; set; }
}