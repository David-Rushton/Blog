namespace Blog.Generator
{
    public class HtmlPathUrl
    {
        public HtmlPathUrl(string path, string url)
            => (Path, Url) = (path, url)
        ;


        public string Path { get; internal set; }
        public string Url { get; internal set; }
    }
}
