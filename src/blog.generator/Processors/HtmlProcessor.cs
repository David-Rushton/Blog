using blog.generator.contexts;
using Markdig;
using System;
using System.IO;
using System.Threading.Tasks;


namespace blog.generator.processors
{
    public class HtmlProcessor : Processor
    {
        readonly MarkdownPipeline _markdownPipeline;


        public HtmlProcessor(MarkdownPipeline markdownPipeline)
            => (_markdownPipeline) = (markdownPipeline)
        ;


        public async override Task InvokeAsync(Context context, NextDelegate next)
        {

            // TODO: Integration with HTML article template

            Console.WriteLine($"Converting article to html: {context.Article.Markdown.Path}");
            context.Article.Html.Content = Markdown.ToHtml(context.Article.Markdown.Content);
            context.Article.Html.Path = Path.ChangeExtension(context.Article.Markdown.Path, ".html");

            Console.WriteLine($"Writing HTML to: {context.Article.Html.Path}");
            using var writer = File.AppendText(context.Article.Html.Path);
            writer.Write(context.Article.Html.Content);

            await next(context);
        }
    }
}
