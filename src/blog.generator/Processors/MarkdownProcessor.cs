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
                .Replace("$(article-content)",      Markdown.ToHtml(context.Markdown.Content, _markdownPipeline))
                .Replace("$(article-author)",       context.Author)
                .Replace("$(article-posted-date)",  GetPostedDate(context))
                .Replace("$(article-title)",        context.Title)
                .Replace("$(article-slug)",         context.Slug)
                .Replace("$(article-image)",        context.Image.Path)
                .Replace("$(article-image-credit)", context.Image.Credit)
            ;

            Console.WriteLine($"Saving content: {context.Html.Path}");
            File.WriteAllText(context.Html.Path, htmlContent);
        }

        public override string ToString()
            => "Markdown Processor"
        ;


        private string GetPostedDate(MarkupContext context)
        {
            var postedDate = context.PostedDate.ToString("yyyy-MM-dd");

            if(context.AgeInDays < 10)
                postedDate += " <span class=\"badge badge-primary\">new</span>";

            return postedDate;
        }
    }
}
