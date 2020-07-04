using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Blog.Generator.Contexts
{
    public class MarkupContextFactory
    {
        readonly string _templateFileName = ".template.html";


        public async Task<List<MarkupContext>> NewMarkupContextsAsync(ScaffoldContext scaffoldContext, string markupRoot)
        {
            var templateContent = await File.ReadAllTextAsync(Path.Join(markupRoot, _templateFileName));
            var markupContextsTask = Directory.GetFiles(markupRoot, "*.md")
                .Select(path =>
                {
                    Func<string, Task<MarkupContext>> GetMarkupContextAsync = async (string path)
                        => new MarkupContext
                        (
                            scaffoldContext,
                            path,
                            await File.ReadAllTextAsync(path),
                            scaffoldContext.ConvertPathToUrl(Path.ChangeExtension(path, "html")),
                            templateContent
                        );

                    return GetMarkupContextAsync(path);
                })
                .ToList()

            ;
            var markupContexts = await Task.WhenAll(markupContextsTask);
            return markupContexts.ToList();
        }
    }
}
