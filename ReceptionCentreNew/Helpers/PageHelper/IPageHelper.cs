namespace ReceptionCentreNew.Helpers.PageHelper;
public interface IPageHelper<T>
{
    IResultSet<T> GetPage(IQueryable<T> items, int pageNumber);
}