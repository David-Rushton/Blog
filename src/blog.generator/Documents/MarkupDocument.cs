using System;


namespace Blog.Generator.Documents
{
    public class MarkupDocument
    {
        public MarkupDocument(string markdownPath)
            => (MarkdownPath, Image) = (markdownPath, new ImageProperties())
        ;


        public string Title { get; set; }
        public string Slug { get; set; }
        public string[] Tags { get; set; }
        public string Author => "David Rushton";
        public DateTime PostedDate { get; set; }
        public ImageProperties Image { get; set; }
        public string HtmlPath { get; set; }
        public string MarkdownPath { get; internal set; }


        public override string ToString()
            => String.Format
            (
                "Title: {0} Slug {1} Tags {2} PostedDate {3} Image Path {4} Image Credit {5} HtmlPath {6} MarkdowPath {7}",
                Title,
                Slug,
                String.Join(", ", Tags),
                PostedDate.ToString("yyyy-MM-dd"),
                Image.Path,
                Image.Credit,
                HtmlPath,
                MarkdownPath
            )
        ;


        public class ImageProperties
        {
            internal ImageProperties()
            { }


            public string Credit { get; set; }
            public string Path { get; set; }
        }
    }
}
