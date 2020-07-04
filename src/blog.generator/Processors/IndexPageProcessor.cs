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
            var index = context.HtmlContexts["/index.html"];
            var articles = context.MarkupContexts
                .OrderByDescending(a => a.PostedDate)
                .Take(5)
                .Select((a, i) => new
                    {
                        a.Title,
                        a.Author,
                        a.PostedDate,
                        a.Slug,
                        a.AgeInDays,
                        ImagePath = a.Image.Url,
                        HtmlUrl = a.Html.Url,
                        Index = i.ToString()
                    })
                .ToList()
            ;


            Console.WriteLine($"Generating index page: {index.Url}");
            index.Content = index.Content
                .Replace("$(last-updated)", DateTime.Now.ToString("yyyy-MM-dd"))
                .Replace("$(version-number)", context.ScaffoldContext.VersionNumber)
            ;

            foreach(var article in articles)
            {
                Console.WriteLine($"\tInserting article preview #{article.Index}: {article.Title}");
                index.Content = index.Content
                    .Replace($"$(article-title-{article.Index})", article.Title)
                    .Replace($"$(article-slug-{article.Index})",  article.Slug)
                    .Replace($"$(article-image-{article.Index})", article.ImagePath)
                    .Replace($"$(article-path-{article.Index})",  article.HtmlUrl)
                ;
            }
        }

        public override string ToString() => "Index Page Processor";
    }
}
