using Blog.Generator.Contexts;
using Blog.Generator.Processors.Abstractions;
using HtmlAgilityPack;
using System;
using System.Text;


namespace Blog.Generator.Processors
{
    public class BlockquoteFormatProcessor : MarkupProcessor
    {
        public override void Invoke(MarkupContext context)
        {
            // Avoid parsing Html if required
            if(MightContainBlockquotes())
            {
                Console.WriteLine($"Formatting blockquotes: {context.Html.Path}");

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(context.Html.Content);

                foreach(var quote in GetBlockquotes(htmlDoc))
                {
                    quote.AddClass("alert");
                    quote.AddClass("alert-primary");
                    quote.Attributes.Add("role", "alert");
                }

                context.Html.Content = htmlDoc.DocumentNode.OuterHtml;
            }


            // Simple text check
            // Can return false positives but not false negatives
            bool MightContainBlockquotes() => context.Html.Content.Contains("<blockquote");

            HtmlNodeCollection GetBlockquotes(HtmlDocument htmlDoc)
            {
                // Markdig converts quotes to: <blockquote><p>**Html-Content-Here**</p></blockquote>
                return htmlDoc.DocumentNode.SelectNodes("//blockquote/p");
            }
        }

        public override string ToString() => "Blockquote Format Processor";
    }
}
