using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
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
            Tags = new List<TagItem>();
            PostedDate = DateTime.MinValue;

            Markdown.Path = markdownPath;
            Markdown.Content = markdownContent;
            Html.Url.RelativeUrl = htmlUrl;
            Html.Path = Path.ChangeExtension(markdownPath, ".html");
            Html.Content = htmlContentTemplate;
        }


        public ScaffoldContext ScaffoldContext { get; internal set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public List<TagItem> Tags { get; set; }
        public string Author => "David Rushton";
        public DateTime PostedDate { get; set; }
        public Double AgeInDays => (DateTime.Now - PostedDate).TotalDays;
        public ImageProperties Image { get; set; }
        public MarkdownProperties Markdown { get; set; }
        public HtmlProperties Html { get; set; }


        /// <summary>
        /// Returns a new tag item
        /// </summary>
        /// <param name="nakedValue">Text value of tag, without any prefix</param>
        /// <returns>TagItem</returns>
        public TagItem NewTagItem(string nakedValue) => new TagItem(nakedValue);

        /// <summary>
        /// Returns the tags, with prefix glyph
        /// </summary>
        /// <param name="separator">Separator used to join tags</param>
        /// <returns>String</returns>
        public string GetDelimitedTags(string separator = " ") => String.Join(separator, Tags.Select(t => t.Value));

        /// <summary>
        /// Returns the tags, without prefix glyph
        /// </summary>
        /// <param name="separator">Separator used to join tags</param>
        /// <returns>String</returns>
        public string GetDelimitedNakedTags(string separator = " ") => String.Join(separator, Tags.Select(t => t.NakedValue));

        /// <summary>
        ///  Returns the PostDate as a string, using a standard format
        /// </summary>
        /// <returns>String</returns>
        public string GetPostedDateAsString() => PostedDate.ToString("yyyy-MM-dd");

        public override string ToString()
            => String.Format
            (
                "Title: {0} Slug {1} Tags {2} Author {3} PostedDate {4} Image Path {5} Image Credit {6}",
                Title,
                Slug,
                String.Join(", ", Tags.Select(t => t.Value)),
                Author,
                PostedDate.ToString("yyyy-MM-dd"),
                Image.Url,
                Image.Credit
            )
        ;


        public class MarkdownProperties
        {
            public MarkdownProperties() => (Path, Content) = ("", "");


            public string Path { get; internal set; }
            public string Content { get; internal set; }
        }


        public class TagItem
        {
            public TagItem(string nakedValue) => (NakedValue) = (nakedValue);


            public const string PrefixGlyph = "🏷️";
            public string Value => $"{PrefixGlyph}{NakedValue}";
            public string NakedValue { get; internal set; }
        }


        public class UrlProperties
        {
            public UrlProperties(string relativeUrl) => RelativeUrl = relativeUrl;


            public string RelativeUrl { get; set; }
            public string AbsoluteUrl => $"https://david-rushton.dev{RelativeUrl}";
        }


        public class HtmlProperties
        {
            private string _content = "";
            private bool _locked = false;


            public HtmlProperties() => (Url, Path, Content) = (new UrlProperties(""), "", "");


            public UrlProperties Url { get; internal set; }
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
            public ImageProperties() => (Credit, Provider, Url) = ("", "", "");


            public string Credit { get; set; }
            public string Provider { get; set; }
            public string Url { get; set; }
        }
    }
}
