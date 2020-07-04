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

            var htmlContent = context.Html.Content
                .Replace("$(article-content)",          Markdown.ToHtml(context.Markdown.Content, _markdownPipeline))
                .Replace("$(article-author)",           context.Author)
                .Replace("$(article-posted-date)",      context.GetPostedDateAsString())
                .Replace("$(article-title)",            context.Title)
                .Replace("$(article-slug)",             context.Slug)
                .Replace("$(article-image)",            context.Image.Url)
                .Replace("$(article-tags)",             context.GetFlattenedTags())
                .Replace("$(article-image-credit)",     context.Image.Credit)
                .Replace("$(article-image-provider)",   context.Image.Provider)
            ;

            context.Html.Content = htmlContent;
        }

        public override string ToString()
            => "Markdown Processor"
        ;
    }
}
