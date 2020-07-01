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
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(File.ReadAllText(html.Path));

                var location = $"https://david-rushton.dev{html.Url}";
                var lastModified = htmlDoc.DocumentNode
                    .SelectSingleNode("/html/head/meta[@name = \"date\"]")
                    .Attributes["content"].Value ?? DateTime.Now.ToString("yyy-MM-dd")
                ;

                sb.AppendLine(GetPageEntry(location, lastModified));
            }

            var content = GetSitemapsTemplate(sb.ToString());
            var sitemapsPath = Path.Join(context.ScaffoldContext.SiteRoot, "sitemaps.xml");
            File.WriteAllText(sitemapsPath, content);
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
