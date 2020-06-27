using System;


namespace blog.generator.contexts
{
    public class ArticleContext
    {
        public ArticleContext(string path, string content, ImageContext imageContext)
        {
            Title = "";
            Slug = "";
            Author = "David Rushton";
            PostedDate = DateTime.MinValue;
            Html = new ContentPath();
            Markdown = new ContentPath
            {
                Path = path,
                Content = content
            };
            PlainText = "";
            Image = imageContext;
        }


        public string Title { get; set; }
        public string Slug { get; set; }
        public string[] Tags { get; set; }
        public string Author { get; set; }
        public DateTime PostedDate { get; set; }
        public ContentPath Html { get; internal set; }
        public ContentPath Markdown { get; internal set; }
        public string PlainText { get; set; }
        public ImageContext Image { get; internal set; }


        public class ContentPath
        {
            internal ContentPath()
                => (Path, Content) = ("", "")
            ;


            public string Path { get; set; }
            public string Content { get; set; }
        }
    }
}
