namespace ReceptionCentreNew.Models;
public class MvcSiteMapNode
{
    public string Title { get; set; }
    public string Controller { get; set; }
    public string Action { get; set; }
    public string IconClass { get; set; }
    public List<MvcSiteMapNode> Children { get; set; } 
}