using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace Blog.Generator.Contexts
{
    /// <summary>
    /// Contains the information required to read, update and write to any of the sites Html pages
    /// </summary>
    public class HtmlContext
    {
        string _content = "";


        public HtmlContext() : this("", "", "")
        { }

        public HtmlContext(string path, string url, string content)
            => (Path, Url, IsDirty, _content) = (path, url, false, content)
        ;


        public string Path { get; internal set; }
        public string Url { get; internal set; }
        public bool IsDirty { get; internal set; }
        public string Content
        {
            get => _content;
            set
            {
                IsDirty = true;
                _content = value;
            }
        }


        public async Task SaveContentAsync() => await File.WriteAllTextAsync(Path, _content, Encoding.UTF8);
    }
}
