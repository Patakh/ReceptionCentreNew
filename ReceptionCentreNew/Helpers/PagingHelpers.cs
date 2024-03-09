using Microsoft.AspNetCore.Mvc.Rendering; 
using ReceptionCentreNew.Models; 
using System.Text;

namespace ReceptionCentreNew;
public static class PagingHelpers
{
    public static TagBuilder PageLinks(this IHtmlHelper html,
        PageInfo pageInfo, Func<PageInfo, string> pageUrl)
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
        StringBuilder result = new ();
        TagBuilder tagUl = new ("ul");
        tagUl.AddCssClass("pagination m-b-0");

        //---B-M---переход на первую страницу
        if (pageInfo.CurrentPage > 3)
        {
            TagBuilder tagLiFirst = new ("li");
            tagLiFirst.InnerHtml.AppendHtml(pageUrl(new PageInfo() { CurrentPage = 1, NameLink = "«" }));
            result.Append(tagLiFirst);
        }

        if (1 != pageInfo.CurrentPage)
        {
            TagBuilder tagLiPrev = new ("li");
            tagLiPrev.InnerHtml.AppendHtml(pageUrl(new PageInfo() { CurrentPage = pageInfo.CurrentPage - 1, NameLink = "‹" }));
            result.Append(tagLiPrev);
        }

        for (int i = pageInfo.FirstPage; i <= pageInfo.LastPage; i++)
        {
            TagBuilder tagLiNum = new("li");
            // если текущая страница, то выделяем ее,
            // например, добавляя класс
            if (i == pageInfo.CurrentPage)
            {
                tagLiNum.AddCssClass("active");
            }
            tagLiNum.InnerHtml.AppendHtml(pageUrl(new PageInfo() { CurrentPage = i, NameLink = i.ToString() }));
            result.Append(tagLiNum);
        }

        if (pageInfo.TotalPages > pageInfo.CurrentPage)
        {
            TagBuilder tagLiLast = new ("li");
            tagLiLast.InnerHtml.AppendHtml(pageUrl(new PageInfo() { CurrentPage = pageInfo.CurrentPage + 1, NameLink = "›" }));
            result.Append(tagLiLast);
        }

        //---B-M---переход на последнюю страницу
        if (pageInfo.CurrentPage < pageInfo.TotalPages - 2)
        {
            TagBuilder tagLiLast = new("li");
            tagLiLast.InnerHtml.AppendHtml(pageUrl(new PageInfo() { CurrentPage = pageInfo.TotalPages, NameLink = "»" }));
            result.Append(tagLiLast);
        }

        tagUl.InnerHtml.AppendHtml(result.ToString());

        return tagUl;
    }
}