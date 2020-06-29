using System;
using System.IO;


namespace Blog.Generator.Contexts
{
    public class MarkupContext
    {
        public MarkupContext()
            : this("", "", "")
        { }

        public MarkupContext(string markdownPath, string markdownContent, string htmlContentTemplate)
            => (MarkdownPath, MarkdownContent, htmlContentTemplate)
            =  (markdownPath, markdownContent, HtmlContentTemplate)
        ;


        public string MarkdownPath { get; internal set; }
        public string MarkdownContent { get; internal set; }
        public string HtmlPath => Path.ChangeExtension(MarkdownPath, ".html");
        public string HtmlContentTemplate { get; internal set; }
    }
}
