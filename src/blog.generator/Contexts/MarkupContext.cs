using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace Blog.Generator.Contexts
{
    /// <summary>
    /// Contains information required to convert raw markup articles in processed Html files
    /// </summary>
    public class MarkupContext
    {
        public MarkupContext(ScaffoldContext scaffoldContext, string markdownPath, string markdownContent, string htmlUrl, string htmlContentTemplate)
        {
            Markdown = new MarkdownProperties();
            Html = new HtmlProperties();
            Image = new ImageProperties();

            ScaffoldContext = scaffoldContext;
            Title = "";
            Slug = "";
            Tags = new string[] { "" };
            PostedDate = DateTime.MinValue;

            Markdown.Path = markdownPath;
            Markdown.Content = markdownContent;
            Html.Url = htmlUrl;
            Html.Path = Path.ChangeExtension(markdownPath, ".html");
            Html.Content = htmlContentTemplate;
        }


        public ScaffoldContext ScaffoldContext { get; internal set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string[] Tags { get; set; }
        public string Author => "David Rushton";
        public DateTime PostedDate { get; set; }
        public Double AgeInDays => (DateTime.Now - PostedDate).TotalDays;
        public ImageProperties Image { get; set; }
        public MarkdownProperties Markdown { get; set; }
        public HtmlProperties Html { get; set; }

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
            private string _content = "";
            private bool _locked = false;


            public HtmlProperties() => (Url, Path, Content) = ("", "", "");


            public string Url { get; internal set; }
            public string Path { get; internal set; }
            public bool IsDirty { get; internal set; }
            public string Content
            {
                get => _content;
                set
                {
                    if(_locked)
                        throw new Exception($"This file has been locked: {Path}\n\tIt can only be updated via the finalise context");

                    IsDirty = true;
                    _content = value;
                }
            }


            public async Task SaveContentAsync() => await File.WriteAllTextAsync(Path, _content, Encoding.UTF8);
            public void LockContent() => _locked = true;
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
