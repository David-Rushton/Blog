using System;
using System.IO;


namespace Blog.Generator.Contexts
{
    public class MarkupContext
    {
        // Used to suppress nullable warnings
        public MarkupContext()
            : this("", "", "", "")
        { }

        public MarkupContext(string markdownPath, string markdownContent, string htmlUrl, string htmlContentTemplate)
        {
            Markdown = new MarkdownProperties();
            Html = new HtmlProperties();
            Image = new ImageProperties();

            Title = "";
            Slug = "";
            Tags = new string[] { "" };
            PostedDate = DateTime.MinValue;

            Markdown.Path = markdownPath;
            Markdown.Content = markdownContent;
            Html.Url = htmlUrl;
            Html.Path = Path.ChangeExtension(markdownPath, ".html");
            Html.ContentTemplate = htmlContentTemplate;
        }


        public string Title { get; set; }
        public string Slug { get; set; }
        public string[] Tags { get; set; }
        public string Author => "David Rushton";
        public DateTime PostedDate { get; set; }
        public Double AgeInDays => (DateTime.Now - PostedDate).TotalDays;
        public ImageProperties Image { get; set; }
        public MarkdownProperties Markdown { get; internal set; }
        public HtmlProperties Html { get; internal set; }

        public override string ToString()
            => String.Format
            (
                "Title: {0} Slug {1} Tags {2} Author {3} PostedDate {4} Image Path {5} Image Credit {6}",
                Title,
                Slug,
                String.Join(", ", Tags),
                Author,
                PostedDate.ToString("yyyy-MM-dd"),
                Image.Path,
                Image.Credit
            )
        ;


        public class MarkdownProperties
        {
            public MarkdownProperties() => (Path, Content) = ("", "");


            public string Path { get; internal set; }
            public string Content { get; internal set; }
        }

        public class HtmlProperties
        {
            public HtmlProperties() => (Url, Path, ContentTemplate, Content) = ("", "", "", "");


            public string Url { get; internal set; }
            public string Path { get; internal set; }
            public string ContentTemplate { get; internal set; }
            public string Content { get; set; }
        }

        public class ImageProperties
        {
            public ImageProperties() => (Credit, Provider, Path) = ("", "", "");


            public string Credit { get; set; }
            public string Provider { get; set; }
            public string Path { get; set; }
        }
    }
}
