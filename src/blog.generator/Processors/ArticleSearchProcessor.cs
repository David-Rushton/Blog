using Blog.Generator.Contexts;
using Blog.Generator.Processors.Abstractions;
using System;
using System.IO;


namespace Blog.Generator.Processors
{
    public class ArticleSearchProcessor : FinaliseProcessor
    {
        const string _searchUrl = "/blog.articles/search.html";


        public override void Invoke(FinaliseContext context)
        {
            var searchPage = context.HtmlContexts[_searchUrl];
        }

        public override string ToString()
            => "Article Search Processor"
        ;




        private string GetSearchCard(
            string articleImage,
            string articleImageCredit,
            string articleImageProvider,
            string articleTitle,
            string articleSlug,
            string articlePostedDate
        )
            => GetSearchCardTemplate()
                .Replace("$(article-image)",            articleImage)
                .Replace("$(article-image-credit)",     articleImageCredit)
                .Replace("$(article-image-provider)",   articleImageProvider)
                .Replace("$(article-title)",            articleTitle)
                .Replace("$(article-slug)",             articleSlug)
                .Replace("$(article-posted-date)",      articlePostedDate)
        ;

        private string GetSearchCardTemplate()
            => @"
                    <div class=""card mb-3"" style=""max-width: 540px;"">
                        <div class=""row no-gutters"">
                            <div class=""col-md-4"">
                            <img src=""$(article-image)"" class=""card-img"" alt=""photo by $(article-image-credit) from $(article-image-provider)"">
                            </div>
                            <div class=""col-md-8"">
                            <div class=""card-body"">
                                <h5 class=""card-title"">$(article-title)</h5>
                                <p class=""card-text text-muted"">$(article-slug)</p>
                                <p class=""card-text""><small class=""text-muted"">posted <strong>$(article-posted-date)</strong></small></p>
                            </div>
                            </div>
                        </div>
                    </div>

            ";
    }
}
