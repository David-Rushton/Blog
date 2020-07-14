using Blog.Generator.Contexts;
using Blog.Generator.Processors.Abstractions;
using Markdig;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;


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
                .Replace("$(article-id)",               context.Id)
                .Replace("$(article-url)",              context.Html.Url.AbsoluteUrl)
                .Replace("$(article-path)",             context.Html.Url.RelativeUrl)
                .Replace("$(article-content)",          Markdown.ToHtml(context.Markdown.Content, _markdownPipeline))
                .Replace("$(article-author)",           context.Author)
                .Replace("$(article-posted-date)",      context.GetPostedDateAsString())
                .Replace("$(article-title)",            context.Title)
                .Replace("$(article-title-encoded)",    HttpUtility.UrlEncode(context.Title))
                .Replace("$(article-slug)",             context.Slug)
                .Replace("$(article-slug-encoded)",     HttpUtility.UrlEncode(context.Slug))
                .Replace("$(article-image)",            context.Image.Url)
                .Replace("$(article-tags)",             context.GetDelimitedTags())
                .Replace("$(article-tags-encoded)",     HttpUtility.UrlEncode(context.GetDelimitedNakedTags(",")))
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
