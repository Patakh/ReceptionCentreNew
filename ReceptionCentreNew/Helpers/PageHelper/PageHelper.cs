namespace ReceptionCentreNew.Helpers.PageHelper;

public class PageHelper<T> : IPageHelper<T>
{
    private readonly IPageConfig _pageConfig;

    public PageHelper(IPageConfig pageConfig)
    {
        _pageConfig = pageConfig;
    }

    public IResultSet<T> GetPage(IQueryable<T> items, int pageNumber)
    {
        var numberOfRecords = items.Count();
        var numberOfPages = GetPaggingCount(numberOfRecords, _pageConfig.PageSize);

        if (pageNumber == 0) { pageNumber = 1; }

        var pager = new Pager
        {
            NumberOfPages = numberOfPages,
            CurrentPage = pageNumber,
            TotalRecords = numberOfRecords
        };

        var countFrom = _countFrom(_pageConfig.PageSize, pageNumber);

        var resultSet = new ResultSet<T>
        {
            Pager = pager,
            Items = items.Skip(countFrom).Take(_pageConfig.PageSize)
        };

        return resultSet;
    }

    private readonly Func<int, int, int> _countFrom =
        (pageSize, pageNumber) => pageNumber == 1 ? 0 : (pageSize * pageNumber) - pageSize;

    private static int GetPaggingCount(int count, int pageSize)
    {
        var extraCount = count % pageSize > 0 ? 1 : 0;
        return (count < pageSize) ? 1 : (count / pageSize) + extraCount;
    }

    public class ResultSet<T> : IResultSet<T>
    {
        public IEnumerable<T> Items { get; set; }
        public Pager Pager { get; set; }
         
    }
}