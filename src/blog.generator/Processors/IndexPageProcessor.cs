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
                        ImagePath = a.Image.Path,
                        HtmlPath = a.Html.Path,
                        Index = i.ToString()
                    })
                .ToList()
            ;
            var indexPath = Path.Join(context.ScaffoldContext.SiteRoot, "index.html");
            var indexContent = File.ReadAllText(indexPath);


            Console.WriteLine($"Generating index page: {indexPath}");
            indexContent = indexContent
                .Replace("$(last-updated)", DateTime.Now.ToString("yyyy-MM-dd"))
                .Replace("$(build-number)", context.ScaffoldContext.BuildNumber)
                .Replace("$(build-sha)",    context.ScaffoldContext.BuildSha)
            ;

            foreach(var article in articles)
            {
                Console.WriteLine($"\tInserting article preview #{article.Index}");
                indexContent = indexContent
                    .Replace($"$(article-title-{article.Index})", article.Author)
                    .Replace($"$(article-slug-{article.Index})",  article.Slug)
                    .Replace($"$(article-image-{article.Index})", article.ImagePath)
                    .Replace($"$(article-path-{article.Index})",  article.HtmlPath)
                ;
            }


            File.WriteAllText(indexPath, indexContent);
        }

        public override string ToString()
            => "Index Page Processor"
        ;
    }
}
