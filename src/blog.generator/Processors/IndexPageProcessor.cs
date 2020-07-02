using Blog.Generator.Contexts;
using Blog.Generator.Processors.Abstractions;
using System;
using System.IO;
using System.Linq;


namespace Blog.Generator.Processors
{
    public class IndexPageProcessor : FinaliseProcessor
    {
        public override void Invoke(FinaliseContext context)
        {
            var articles = context.MarkupContexts
                .OrderByDescending(a => a.PostedDate)
                .Select((a, i) => new
                    {
                        a.Title,
                        a.Author,
                        a.PostedDate,
                        a.Slug,
                        a.AgeInDays,
                        ImagePath = a.Image.Path,
                        HtmlUrl = a.Html.Url,
                        IndexStr = i.ToString(),
                        IndexInt = i
                    })
                .ToList()
            ;
            var indexPath = Path.Join(context.ScaffoldContext.SiteRoot, "index.html");
            var indexContent = File.ReadAllText(indexPath);


            Console.WriteLine($"Generating index page: {indexPath}");
            indexContent = indexContent
                .Replace("$(last-updated)", DateTime.Now.ToString("yyyy-MM-dd"))
                .Replace("$(version-number)", context.ScaffoldContext.VersionNumber)
            ;

            foreach(var article in articles)
            {
                Console.WriteLine($"\tInserting article preview #{article.IndexStr}");
                indexContent = indexContent
                    .Replace($"$(article-title-{article.IndexStr})", GetTitle(article.IndexInt, article.Title, article.AgeInDays))
                    .Replace($"$(article-slug-{article.IndexStr})",  article.Slug)
                    .Replace($"$(article-image-{article.IndexStr})", article.ImagePath)
                    .Replace($"$(article-path-{article.IndexStr})",  article.HtmlUrl)
                ;
            }


            File.WriteAllText(indexPath, indexContent);
        }

        public override string ToString()
            => "Index Page Processor"
        ;


        // TODO: Switch to NewBadgeProcessor.
        private string GetTitle(int index, string title, double ageInDays)
        {
            // No need to tag the lead article as new
            if(ageInDays < 10 && index > 0)
                title += " <span class=\"badge badge-primary\">new</span>";

            return title;
        }
    }
}
