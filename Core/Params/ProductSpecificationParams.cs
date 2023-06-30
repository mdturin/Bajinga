namespace Core.Params;

public class ProductSpecificationParams
{
    private const int _maxPageSize = 50;

    public string Sort { get; set; } = string.Empty;
    public int? TypeId { get; set; }
    public int? BrandId { get; set; }
    public int PageIndex { get; set; } = 1;

    private int _pageSize = 6;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
    }

    private string _search = string.Empty;
    public string Search
    {
        get => _search;
        set => _search = value.ToLower();
    }
}
