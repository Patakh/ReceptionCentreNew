using ReceptionCentreNew.Helpers.PageHelper;

namespace ReceptionCentreNew.Helpers;
public interface IPageResult<T> where T : class
{
    IEnumerable<T> Items { get; set; }

    Page Pager { get; }
}