namespace poczta;

public interface IResponseBase
{
    void SetLinks(IEnumerable<LinkObject> links);
}