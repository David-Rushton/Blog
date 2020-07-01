using Blog.Generator.Contexts;
using Blog.Generator.Processors.Abstractions;
using HtmlAgilityPack;
using System;
using System.IO;
using System.Text;


namespace Blog.Generator.Processors
{
    public class SitemapsProcessor : FinaliseProcessor
    {
        public override void Invoke(FinaliseContext context)
        {
            var sb = new StringBuilder();

            // Every .html page is added to the sitemaps file.
            foreach(var html in context.Html)
            {
                if(html.Path.EndsWith(".template.html"))
                    continue;

                var location = $"https://david-rushton.dev{html.Url}";
                var lastModified = ExtractLastModified(html.Path);

                sb.AppendLine(GetPageEntry(location, lastModified));
            }


            // Write sitemaps to file system
            WriteSitemaps(context.ScaffoldContext.SiteRoot, sb.ToString());


            string ExtractLastModified(string htmlPath)
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(File.ReadAllText(htmlPath));

                return htmlDoc.DocumentNode
                    .SelectSingleNode("/html/head/meta[@name = \"date\"]")
                    .Attributes["content"].Value
                    ?? DateTime.Now.ToString("yyy-MM-dd")
                ;
            }

            void WriteSitemaps(string path, string urls)
            {
                var content = GetSitemapsTemplate(urls);
                var sitemapsPath = Path.Join(path, "sitemaps.xml");
                File.WriteAllText(sitemapsPath, content);
                Console.WriteLine("Sitemaps.xml created");
            }
        }

        public override string ToString()
            => "Sitemaps Processor"
        ;

        private string GetSitemapsTemplate(string urls)
            => $@"<?xml version=""1.0"" encoding=""utf-8""?>
<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9""
        xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
        xsi:schemaLocation=""http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd"">
    {urls}
</urlset>
";

        private string GetPageEntry(string location, string lastModified)
            =>$@"
    <url>
        <loc>{location}</loc>
        <lastmod>{lastModified}</lastmod>
    </url>
";
    }
}
