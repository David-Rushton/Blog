using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Blog.Generator.Contexts
{
    public class FinaliseContextFactor
    {
        public async Task<FinaliseContext> NewFinaliseContextAsync(
            ScaffoldContext scaffoldContext,
            List<MarkupContext> markupContexts
        )
        {
            var htmlContextsTask = Directory.GetFiles
                (
                    scaffoldContext.SiteRoot,
                    "*.html",
                    new EnumerationOptions
                    {
                        RecurseSubdirectories = true
                    }
                )
                .Select(path =>
                {
                    Func<string, Task<HtmlContext>> GetHtmlContext = async (string path)
                        => new HtmlContext
                        {
                            Path = path,
                            Url = scaffoldContext.ConvertPathToUrl(path),
                            Content = await File.ReadAllTextAsync(path)
                        };

                   return GetHtmlContext(path);
                }
                )
                .ToList()
            ;

            var htmlContexts = await Task.WhenAll(htmlContextsTask);
            var htmlContextsDict = htmlContexts.ToDictionary(c => c.Url);

            return new FinaliseContext(scaffoldContext, markupContexts, htmlContextsDict);
        }
    }
}
