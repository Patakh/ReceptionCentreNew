namespace ReceptionCentreNew.Models;
public class PageInfo
{
    public int TotalItems { get; set; }
    public int NumberRow { get; set; }
    public int ItemsPerPage { get; set; }
    public int CurrentPage { get; set; }
    public int FirstPage { get; set; }
    public int LastPage { get; set; }
    public int MaxPageList { get; set; }
    public string Action { get; set; }
    public string UpdateTarget { get; set; }
    public string View { get; set; }
    public string Controller { get; set; }
    public string NameLink { get; set; }
    public int TotalPages
    {
        get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
    }
}