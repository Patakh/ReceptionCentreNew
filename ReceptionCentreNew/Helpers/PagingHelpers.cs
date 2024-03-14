using ReceptionCentreNew.Models;  
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering; 
using System.Text;

namespace ReceptionCentreNew.Helpers;
public static class PagingHelpers
{
    public static IHtmlContent PageLinks(this IHtmlHelper html,
        PageInfo pageInfo, Func<PageInfo, IHtmlContent> pageUrl)
    {
        pageInfo.FirstPage = (int)(pageInfo.CurrentPage - (int)(pageInfo.MaxPageList / 2));
        if (pageInfo.FirstPage <= 1)
        {
            pageInfo.FirstPage = 1;
        }
        else
        {
            if (pageInfo.TotalPages - pageInfo.FirstPage < pageInfo.MaxPageList)
            {
                pageInfo.FirstPage = pageInfo.TotalPages - pageInfo.MaxPageList + 1;
                if (pageInfo.FirstPage <= 1)
                {
                    pageInfo.FirstPage = 1;
                }
            }
        }
        pageInfo.LastPage = pageInfo.FirstPage + pageInfo.MaxPageList - 1;
        if (pageInfo.LastPage > pageInfo.TotalPages)
        {
            pageInfo.LastPage = pageInfo.TotalPages;
        }
        StringBuilder result = new StringBuilder();
        TagBuilder tagUl = new TagBuilder("ul");
        tagUl.AddCssClass("pagination m-b-0");

        if (pageInfo.CurrentPage > 3)
        {
            TagBuilder tagLiFirst = new TagBuilder("li");
            tagLiFirst.InnerHtml.AppendHtml(pageUrl(new PageInfo() { CurrentPage = 1, NameLink = "«" }));
            result.Append(tagLiFirst.ToString());
        }

        if (1 != pageInfo.CurrentPage)
        {
            TagBuilder tagLiPrev = new TagBuilder("li");
            tagLiPrev.InnerHtml.AppendHtml(pageUrl(new PageInfo() { CurrentPage = pageInfo.CurrentPage - 1, NameLink = "‹" }));
            result.Append(tagLiPrev.ToString());
        }

        for (int i = pageInfo.FirstPage; i <= pageInfo.LastPage; i++)
        {
            TagBuilder tagLiNum = new TagBuilder("li");
            if (i == pageInfo.CurrentPage)
            {
                tagLiNum.AddCssClass("active");
            }
            tagLiNum.InnerHtml.AppendHtml(pageUrl(new PageInfo() { CurrentPage = i, NameLink = i.ToString() }));
            result.Append(tagLiNum.ToString());
        }

        if (pageInfo.TotalPages > pageInfo.CurrentPage)
        {
            TagBuilder tagLiLast = new TagBuilder("li");
            tagLiLast.InnerHtml.AppendHtml(pageUrl(new PageInfo() { CurrentPage = pageInfo.CurrentPage + 1, NameLink = "›" }));
            result.Append(tagLiLast.ToString());
        }

        if (pageInfo.CurrentPage < pageInfo.TotalPages - 2)
        {
            TagBuilder tagLiLast = new TagBuilder("li");
            tagLiLast.InnerHtml.AppendHtml(pageUrl(new PageInfo() { CurrentPage = pageInfo.TotalPages, NameLink = "»" }));
            result.Append(tagLiLast.ToString());
        }

        tagUl.InnerHtml.AppendHtml(result.ToString());
        return new HtmlString(tagUl.ToString());
    }
}