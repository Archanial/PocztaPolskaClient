namespace poczta;

public sealed class Response<T> : IResponseBase
{
    public required T Content { get; set; }
    
    public List<LinkObject> Links { get; set; } = [];
    
    public void SetLinks(IEnumerable<LinkObject> links) => Links = links.ToList();
}