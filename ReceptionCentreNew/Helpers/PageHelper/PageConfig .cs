namespace ReceptionCentreNew.Helpers.PageHelper;
public class PageConfig : IPageConfig
{
    private readonly IConfiguration _configuration;

    public PageConfig(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public int PageSize
    {
        get
        {
            int.TryParse(_configuration["Pager:PageSize"], out int val);
            return val < 1 ? 20 : val;
        }
    }
}