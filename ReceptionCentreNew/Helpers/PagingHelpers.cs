
using Microsoft.AspNetCore.Html; 
using ReceptionCentreNew.Models; 
using System.Text;
using System.Web.Mvc;
namespace ReceptionCentreNew;
public static class PagingHelpers
{
    public static HtmlString PageLinks(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper html, PageInfo pageInfo, Func<PageInfo, HtmlString> pageUrl)
    {
        pageInfo.FirstPage = pageInfo.CurrentPage - pageInfo.MaxPageList / 2;
        if (pageInfo.FirstPage <= 1)
        {
            pageInfo.FirstPage = 1;
        }
        else if (pageInfo.TotalPages - pageInfo.FirstPage < pageInfo.MaxPageList)
        {
            pageInfo.FirstPage = pageInfo.TotalPages - pageInfo.MaxPageList + 1;
            if (pageInfo.FirstPage <= 1)
            {
                pageInfo.FirstPage = 1;
            }
        }

        pageInfo.LastPage = pageInfo.FirstPage + pageInfo.MaxPageList - 1;
        if (pageInfo.LastPage > pageInfo.TotalPages)
        {
            pageInfo.LastPage = pageInfo.TotalPages;
        }

        StringBuilder stringBuilder = new StringBuilder();
        TagBuilder tagBuilder = new TagBuilder("ul");
        tagBuilder.AddCssClass("pagination m-b-0");
        if (pageInfo.CurrentPage > 3)
        {
            TagBuilder tagBuilder2 = new TagBuilder("li");
            tagBuilder2.InnerHtml = pageUrl(new PageInfo
            {
                CurrentPage = 1,
                NameLink = "«"
            }).ToString();
            stringBuilder.Append(tagBuilder2.ToString());
        }

        if (1 != pageInfo.CurrentPage)
        {
            TagBuilder tagBuilder3 = new TagBuilder("li");
            tagBuilder3.InnerHtml = pageUrl(new PageInfo
            {
                CurrentPage = pageInfo.CurrentPage - 1,
                NameLink = "‹"
            }).ToString();
            stringBuilder.Append(tagBuilder3.ToString());
        }

        for (int i = pageInfo.FirstPage; i <= pageInfo.LastPage; i++)
        {
            TagBuilder tagBuilder4 = new TagBuilder("li");
            if (i == pageInfo.CurrentPage)
            {
                tagBuilder4.AddCssClass("active");
            }

            tagBuilder4.InnerHtml = pageUrl(new PageInfo
            {
                CurrentPage = i,
                NameLink = i.ToString()
            }).ToString();
            stringBuilder.Append(tagBuilder4.ToString());
        }

        if (pageInfo.TotalPages > pageInfo.CurrentPage)
        {
            TagBuilder tagBuilder5 = new TagBuilder("li");
            tagBuilder5.InnerHtml = pageUrl(new PageInfo
            {
                CurrentPage = pageInfo.CurrentPage + 1,
                NameLink = "›"
            }).ToString();
            stringBuilder.Append(tagBuilder5.ToString());
        }

        if (pageInfo.CurrentPage < pageInfo.TotalPages - 2)
        {
            TagBuilder tagBuilder6 = new TagBuilder("li");
            tagBuilder6.InnerHtml = pageUrl(new PageInfo
            {
                CurrentPage = pageInfo.TotalPages,
                NameLink = "»"
            }).ToString();
            stringBuilder.Append(tagBuilder6.ToString());
        }

        tagBuilder.InnerHtml = stringBuilder.ToString();

        return new HtmlString(tagBuilder.ToString());
    }
}