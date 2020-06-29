using Blog.Generator.Contexts;
using Blog.Generator.Documents;
using Blog.Generator.Processors.Abstractions;
using Markdig;
using System;
using System.IO;
using System.Threading.Tasks;


namespace Blog.Generator.Processors
{
    public class MarkdownProcessor : MarkupProcessor
    {
        readonly MarkdownPipeline _markdownPipeline;

        public MarkdownProcessor(MarkdownPipeline markdownPipeline)
            => (_markdownPipeline) = (markdownPipeline)
        ;


        public override void Invoke(MarkupContext context, MarkupDocument markupDocument)
        {
            Console.WriteLine($"Converting markdown to Html: {context.MarkdownPath}");

            var htmlContent = context.HtmlContentTemplate
                .Replace("$(article-author)",       markupDocument.Author)
                .Replace("$(article-posted-date)",  markupDocument.PostedDate.ToString("yyyy-MM-dd"))
                .Replace("$(article-title)",        markupDocument.Title)
                .Replace("$(article-slug)",         markupDocument.Slug)
                .Replace("$(article-image)",        markupDocument.Image.Path)
                .Replace("$(article-image-credit)", markupDocument.Image.Credit)
                .Replace("$(article-content)",      context.MarkdownContent)
            ;

            Console.WriteLine($"Saving content: {context.HtmlPath}");
            File.WriteAllText(context.HtmlPath, htmlContent);
        }
    }
}
