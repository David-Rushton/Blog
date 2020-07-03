using Blog.Generator.Contexts;
using Blog.Generator.Processors.Abstractions;
using System;
using System.Linq;


namespace Blog.Generator.Processors
{
    public class ArticleNavigationProcessor : FinaliseProcessor
    {
        public override void Invoke(FinaliseContext context)
        {
            Console.WriteLine($"Building article navigations buttons");

            var articleMax = context.MarkupContexts.Count - 1;
            var markups = context.MarkupContexts.OrderBy(a => a.PostedDate).ToList();
            for(var i = 0; i <= articleMax; i++)
            {
                var previousDisabled = i == 0 ? "disabled" : "";
                var previousPath = i == 0 ? "#" : markups[i - 1].Html.Url;
                var nextDisabled = i == articleMax ? "disabled" : "";
                var nextPath = i == articleMax ? "#" : markups[i + 1].Html.Url;


                // Markups.Html context contains all the information we need to implement navigation buttons
                // However this context is read-only during finalise processing.
                // Instead we have to look up the Html context for the same file.
                context.HtmlContexts[markups[i].Html.Url].Content = markups[i].Html.Content
                    .Replace("$(disable-previous-article-navigation)", previousDisabled)
                    .Replace("$(previous-article-path)", previousPath)
                    .Replace("$(disable-next-article-navigation)", nextDisabled)
                    .Replace("$(next-article-path)", nextPath)
                ;
            }
        }

        public override string ToString()
            => "Article Navigation Processor"
        ;
    }
}
