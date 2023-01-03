namespace Shared.RequestFeatures;

public abstract class RequestParameters
{
    private const int maxPageSize = 50;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; private set; }

    public void SetPageSize(int pageSize)
    {
        PageSize = pageSize > maxPageSize ? maxPageSize : pageSize;
    }
}