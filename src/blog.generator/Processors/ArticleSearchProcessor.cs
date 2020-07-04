using Blog.Generator.Contexts;
using Blog.Generator.Processors.Abstractions;
using System;
using System.Text;
using System.Linq;


namespace Blog.Generator.Processors
{
    public class ArticleSearchProcessor : FinaliseProcessor
    {
        const string _searchUrl = "/blog.articles/search.html";


        public override void Invoke(FinaliseContext context)
        {
            Console.WriteLine("Generating article search page:");
            var searchPage = context.HtmlContexts[_searchUrl];

            PopulateSearchByDate(searchPage, context);
            PopulateSearchByTag(searchPage, context);
        }

        public override string ToString()
            => "Article Search Processor"
        ;


        private void PopulateSearchByTag(HtmlContext searchPage, FinaliseContext context)
        {
            var searchTable = new StringBuilder();
            var searchList = new StringBuilder();
            var searchByTag =
                from article in context.MarkupContexts
                from tag in article.Tags
                group article by tag into articleTags
                orderby articleTags.Key
                select new
                {
                    Tag = articleTags.Key,
                    TagId = $"tag-{articleTags.Key.Substring(3)}",
                    Articles = articleTags.ToList(),
                    ArticlesCount = articleTags.Sum(a=>1)
                }
            ;


            searchTable.Append("<div class=\"list-group\" style=\"max-width: 600px;\">");

            // To avoid iterating over the collection twice, we build the search table and the search list together.
            // The search table contains one row per tag and links to the various sections of the search list.
            // The search list appears below the search table and has tag per section.  Each section contains a series
            // of cards linking to the various articles with that tag.  Each article can appear in multiple sections.
            foreach(var tag in searchByTag)
            {
                Console.WriteLine($"\tInserting search entry for tag: {tag.Tag}");

                searchTable.Append(GetSearchTableRow(tag.TagId, tag.Tag, tag.ArticlesCount));

                searchList.Append($"<h3 id=\"{tag.TagId}\"><code>{tag.Tag}</code></h3>");
                foreach(var article in tag.Articles)
                {
                    searchList.Append(GetSearchCard(
                        article.Image.Url,
                        article.Image.Credit,
                        article.Image.Provider,
                        article.Title,
                        article.Slug,
                        article.GetPostedDateAsString(),
                        article.Html.Url
                    ));
                }
            }

            searchTable.Append("</div><hr>\n");
            searchTable.Append(searchList.ToString());

            searchPage.Content = searchPage.Content.Replace("$(search-by-tag)", searchTable.ToString());
        }

        private void PopulateSearchByDate(HtmlContext searchPage, FinaliseContext context)
        {
            var searchByDate = context.MarkupContexts.OrderByDescending(c => c.PostedDate).ToList();
            var currentPeriod = searchByDate[0].PostedDate.ToString("MMMM yyyy");
            var lastPeriod = currentPeriod;
            var sb = new StringBuilder($"<h1>{currentPeriod}</h1>");

            foreach(var article in searchByDate)
            {
                Console.WriteLine($"\tInserting search entry for: {article.Title}");
                currentPeriod = article.PostedDate.ToString("MMMM yyyy");

                if(currentPeriod != lastPeriod)
                {
                    lastPeriod = currentPeriod;
                    sb.Append($"<h1>{currentPeriod}</h1>");
                }

                sb.Append(GetSearchCard(
                    article.Image.Url,
                    article.Image.Credit,
                    article.Image.Provider,
                    article.Title,
                    article.Slug,
                    article.GetPostedDateAsString(),
                    article.Html.Url
                ));
            }

            searchPage.Content = searchPage.Content.Replace("$(search-by-post-date)", sb.ToString());
        }


        private string GetSearchTableRow(string tagId, string tag, int articlesCount)
            => @"
                <a href=""#$(tag-id)"" class=""list-group-item list-group-item-action d-flex justify-content-between"">
                    <code>$(tag)</code>
                    <span class=""badge badge-primary badge-pill"">$(articles-count)</span>
                </a>
            "
            .Replace("$(tag-id)",           tagId)
            .Replace("$(tag)",              tag)
            .Replace("$(articles-count)",   articlesCount.ToString("#,0"))
        ;

        private string GetSearchCard(
            string articleImage,
            string articleImageCredit,
            string articleImageProvider,
            string articleTitle,
            string articleSlug,
            string articlePostedDate,
            string articleUrl
        )
            => GetSearchCardTemplate()
                .Replace("$(article-image)",            articleImage)
                .Replace("$(article-image-credit)",     articleImageCredit)
                .Replace("$(article-image-provider)",   articleImageProvider)
                .Replace("$(article-title)",            articleTitle)
                .Replace("$(article-slug)",             articleSlug)
                .Replace("$(article-posted-date)",      articlePostedDate)
                .Replace("$(article-url)",              articleUrl)
        ;

        private string GetSearchCardTemplate()
            => @"
                    <div class=""card mb-3"" style=""max-width: 600px;"">
                        <div class=""row no-gutters"">
                            <div class=""col-md-4"">
                                <a href=""$(article-url)"">
                                    <img src=""$(article-image)"" class=""card-img"" style=""height: 100%; "" alt=""photo by $(article-image-credit) from $(article-image-provider)"">
                                </a>
                            </div>
                            <div class=""col-md-8"">
                            <div class=""card-body"">
                                <a href=""$(article-url)"">
                                    <h5 class=""card-title"">$(article-title)</h5>
                                </a>
                                <p class=""card-text text-muted"">$(article-slug)</p>
                                <p class=""card-text""><small class=""text-muted"">posted <strong>$(article-posted-date)</strong></small></p>
                            </div>
                            </div>
                        </div>
                    </div>

            ";
    }
}
