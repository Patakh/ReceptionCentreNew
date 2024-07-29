namespace ReceptionCentreNew.Helpers.PageHelper;
public interface IPageHelper<T> where T : class
{
    IPageResult<T> GetPage(IQueryable<T> items, int pageNumber, int rowCount);
}