using Blog.Generator.Contexts;
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


        public override void Invoke(MarkupContext context)
        {
            Console.WriteLine($"Converting markdown to Html: {context.Markdown.Path}");

            var htmlContent = context.Html.ContentTemplate
                .Replace("$(article-content)",          Markdown.ToHtml(context.Markdown.Content, _markdownPipeline))
                .Replace("$(article-author)",           context.Author)
                .Replace("$(article-posted-date)",      context.PostedDate.ToString("yyyy-MM-dd"))
                .Replace("$(article-title)",            context.Title)
                .Replace("$(article-slug)",             context.Slug)
                .Replace("$(article-image)",            context.Image.Path)
                .Replace("$(article-image-credit)",     context.Image.Credit)
                .Replace("$(article-image-provider)",   context.Image.Provider)
            ;

            Console.WriteLine($"Saving content: {context.Html.Path}");
            context.Html.Content = htmlContent;
        }

        public override string ToString()
            => "Markdown Processor"
        ;
    }
}
