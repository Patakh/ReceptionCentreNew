using ReceptionCentreNew.Helpers;
using ReceptionCentreNew.Helpers.PageHelper;

namespace ReceptionCentreNew.Models;

public class PageViewModel<T> : IPageResult<T> where T : class
{
    public IEnumerable<T> Items { get; set; }
    public Page Pager { get; set; }
}