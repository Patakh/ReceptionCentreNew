using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Helpers.PageHelper;

namespace ReceptionCentreNew.Models;

public class PagerViewModel<T>
{
    public IEnumerable<T> Items { get; set; }
    public Pager Pager { get; set; }
}