using Microsoft.EntityFrameworkCore;
using ReceptionCentreNew.Models;

namespace ReceptionCentreNew.Helpers.PageHelper;

public class PageHelper<T> : IPageHelper<T> where T : class
{
    public IPageResult<T> GetPage(IQueryable<T> items, int pageNumber, int rowCount)
    {
        var numberOfRecords = items.Count();
        var numberOfPages = GetPaggingCount(numberOfRecords, rowCount);

        if (pageNumber == 0) { pageNumber = 1; }

        var pager = new Page
        {
            NumberOfPages = numberOfPages,
            CurrentPage = pageNumber,
            TotalRecords = numberOfRecords
        };

        var countFrom = _countFrom(rowCount, pageNumber);

        var resultSet = new PageViewModel<T>
        {
            Pager = pager,
            Items = items.Skip(countFrom).Take(rowCount)
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
}