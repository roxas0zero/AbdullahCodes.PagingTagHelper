using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbdullahCodes.PagingTagHelper
{
    public class PagingTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;
        private int TotalPages;
        private int Start;
        private int End;

        public PagingTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            this.urlHelperFactory = urlHelperFactory;
        }

        [ViewContext]
        public ViewContext ViewContext { get; set; }
        public string PageAction { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public bool AlignCenter { get; set; } = true;
        public int MaxDisplayedPages { get; set; } = 10;
        public bool Responsive { get; set; } = true;
        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; }
            = new Dictionary<string, object>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TotalPages = (int)Math.Ceiling(TotalRecords / (double)PageSize);

            if (TotalPages > 1)
            {
                var pagingTag = new TagBuilder("ul");
                pagingTag.AddCssClass("pagination");

                if (AlignCenter)
                {
                    pagingTag.AddCssClass("justify-content-center");
                }

                if (Responsive)
                {
                    pagingTag.AddCssClass("d-flex flex-wrap");
                }

                var prevPageNumber = PageNumber - 1 <= 1 ? 1 : PageNumber - 1;
                var prevTag = CreatePagingLink(prevPageNumber, "&lsaquo; Previous", "disabled");
                pagingTag.InnerHtml.AppendHtml(prevTag);

                if (MaxDisplayedPages == 1)
                {
                    var pageNumberTag = CreatePagingLink(PageNumber, null, "active");
                    pagingTag.InnerHtml.AppendHtml(pageNumberTag);
                }
                else if (MaxDisplayedPages > 1)
                {
                    CalculateBoundaries(PageNumber, TotalPages, MaxDisplayedPages);

                    string gapTag = "<li class=\"page-item border-0\">&nbsp;...&nbsp;</li>";

                    if (End > MaxDisplayedPages)
                    {
                        var firstPageTag = CreatePagingLink(1, null, "active");
                        pagingTag.InnerHtml.AppendHtml(firstPageTag);
                        pagingTag.InnerHtml.AppendHtml(gapTag);
                    }

                    for (int i = Start; i <= End; i++)
                    {
                        var numberTag = CreatePagingLink(i, null, "active");
                        pagingTag.InnerHtml.AppendHtml(numberTag);
                    }

                    if (End < TotalPages)
                    {
                        pagingTag.InnerHtml.AppendHtml(gapTag);
                        var lastPageTag = CreatePagingLink(TotalPages, null, "active");
                        pagingTag.InnerHtml.AppendHtml(lastPageTag);
                    }
                }

                var nextPageNumber = PageNumber + 1 > TotalPages ? TotalPages : PageNumber + 1;
                var nextTag = CreatePagingLink(nextPageNumber, "Next &rsaquo;", "disabled");
                pagingTag.InnerHtml.AppendHtml(nextTag);

                output.TagName = "nav";
                output.Content.AppendHtml(pagingTag);
            }
        }

        private void CalculateBoundaries(int currentPageNumber, int totalPages, int maxDisplayedPages)
        {
            int start, end;

            int gap = (int)Math.Ceiling(maxDisplayedPages / 2.0);

            if (maxDisplayedPages > totalPages)
                maxDisplayedPages = totalPages;

            if (totalPages == 1)
            {
                start = end = 1;
            }
            else if (currentPageNumber < maxDisplayedPages)
            {
                start = 1;
                end = maxDisplayedPages;
            }
            else if (currentPageNumber + maxDisplayedPages == totalPages)
            {
                start = totalPages - maxDisplayedPages > 0 ? totalPages - maxDisplayedPages - 1 : 1;
                end = totalPages - 2;
            }
            else if (currentPageNumber + maxDisplayedPages == totalPages + 1)
            {
                start = totalPages - maxDisplayedPages > 0 ? totalPages - maxDisplayedPages : 1;
                end = totalPages - 1;
            }
            else if (currentPageNumber + maxDisplayedPages > totalPages + 1)
            {
                start = totalPages - maxDisplayedPages > 0 ? totalPages - maxDisplayedPages + 1 : 1;
                end = totalPages;
            }
            else
            {
                start = currentPageNumber - gap > 0 ? currentPageNumber - gap + 1 : 1;
                end = start + maxDisplayedPages - 1;
            }

            Start = start;
            End = end;
        }

        private TagBuilder CreatePagingLink(int targetPageNumber, string text, string pClass)
        {
            var liTag = new TagBuilder("li");
            liTag.AddCssClass("page-item");

            PageUrlValues["page"] = targetPageNumber;
            string pageUrl;
            if (PageAction == null)
            {
                pageUrl = $"?{string.Join("&", PageUrlValues.Select(u => $"{u.Key}={u.Value}"))}";
            }
            else
            {
                IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
                pageUrl = urlHelper.Action(PageAction, PageUrlValues);
            }
            var aTag = new TagBuilder("a");
            aTag.AddCssClass("page-link");
            aTag.Attributes.Add("href", pageUrl);

            if (string.IsNullOrWhiteSpace(text))
            {
                aTag.InnerHtml.Append($"{targetPageNumber}");
            }
            else
            {
                aTag.InnerHtml.AppendHtml($"<span>{text}</span>");
            }

            if (PageNumber == targetPageNumber)
            {
                liTag.AddCssClass($"{pClass}");
                aTag.Attributes.Add("tabindex", "-1");
                aTag.Attributes.Remove("href");
            }

            liTag.InnerHtml.AppendHtml(aTag);
            return liTag;
        }
    }
}