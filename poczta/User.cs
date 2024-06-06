using System.Text.Json.Serialization;

namespace poczta;

public sealed class User
{
    public int Id { get; set; }
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public required string Username { get; set; }

    [JsonIgnore]
    public required string Password { get; set; }

}