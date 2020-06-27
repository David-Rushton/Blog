using blog.generator.contexts;
using Markdig;
using System;
using System.IO;
using System.Threading.Tasks;


namespace blog.generator.processors
{
    public class MarkdownProcessor : Processor
    {
        readonly MarkdownPipeline _markdownPipeline;
        readonly string _htmlTemplate;


        public MarkdownProcessor(MarkdownPipeline markdownPipeline, string htmlTemplate)
            => (_markdownPipeline, _htmlTemplate) = (markdownPipeline, htmlTemplate)
        ;


        public async override Task InvokeAsync(Context context, NextDelegate next)
        {
            var task = Task.Run(async () => {
                Console.WriteLine($"Converting markdown to Html: {context.Article.Markdown.Path}");

                var htmlPath = Path.ChangeExtension(context.Article.Markdown.Path, "html");
                var htmlContent = _htmlTemplate
                    .Replace("$(article-author)", context.Article.Author)
                    .Replace("$(article-posted-date)", context.Article.PostedDate.ToString("yyyy-mm-dd"))
                    .Replace("$(article-title)", context.Article.Title)
                    .Replace("$(article-slug)", context.Article.Slug)
                    .Replace("$(article-image)", context.Article.Image.Path)
                    .Replace("$(article-image-credit)", context.Article.Image.CreditHtml)
                    .Replace("$(article-content)", Markdown.ToHtml(context.Article.Markdown.Content))
                ;

                Console.WriteLine($"Save content: {htmlPath}");
                var htmlWriter = File.WriteAllTextAsync(htmlPath, htmlContent);

                context.Article.Html.Content = htmlContent;
                context.Article.Html.Path = htmlPath;
                context.Article.PlainText = Markdown.ToPlainText(context.Article.Markdown.Content);

                await htmlWriter;
                await next(context);
            });

            await task;
        }
    }
}
