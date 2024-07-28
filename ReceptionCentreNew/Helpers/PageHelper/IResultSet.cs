using ReceptionCentreNew.Helpers.PageHelper;

namespace ReceptionCentreNew.Helpers;
public interface IResultSet<T>
{
    IEnumerable<T> Items { get; set; }

    Pager Pager { get; }
}