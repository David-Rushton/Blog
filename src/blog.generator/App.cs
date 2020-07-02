using Blog.Generator.Contexts;
using Blog.Generator.Processors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Blog.Generator
{
    public class App
    {
        readonly Config _config;
        readonly ContextBuilder _contextBuilder;
        readonly ProcessorPipeline _processorPipeline;


        public App(Config config, ContextBuilder contextBuilder, ProcessorPipeline processorPipeline)
        {
            _config = config;
            _contextBuilder = contextBuilder;
            _processorPipeline = processorPipeline;
        }


        public async Task InvokeAsync()
        {
            Console.WriteLine("Generating blog...");
            Console.WriteLine(_config);
            Console.WriteLine(_processorPipeline);


            // scaffolding
            var scaffoldContext = _contextBuilder.GetScaffoldContext();
            _processorPipeline.InvokeScaffoldPipeline(scaffoldContext);


            // markup to html
            var templateHtml = await File.ReadAllTextAsync(Path.Join(_config.ArticlesTargetRoot, ".template.html"));
            foreach(var path in Directory.GetFiles(_config.ArticlesTargetRoot, "*.md"))
            {
                var content = File.ReadAllText(path);
                var url = ConvertPathToUrl(path);
                var markupContext = _contextBuilder.BuildMarkupContext(path, content, url, templateHtml);

                _processorPipeline.InvokeMarkupPipeline(markupContext);
            }


            // finalising
            var htmlFilePaths = Directory.GetFiles
                (
                    _config.BlogRoot, "*.html",
                    new EnumerationOptions
                    {
                        RecurseSubdirectories = true
                    }
                )
                .Select(p => new HtmlPathUrl(p, ConvertPathToUrl(p)))
                .ToList()
            ;
            var finalisingContext = _contextBuilder.BuildFinaliseContext(htmlFilePaths);
            _processorPipeline.InvokeFinalisePipeline(finalisingContext);


            // Save
            var markupFiles = finalisingContext.MarkupContexts
                .Select(a => File.WriteAllTextAsync(a.Html.Path, a.Html.Content))
                .ToList()
            ;


            await Task.WhenAll(markupFiles);


            OutputSuccessMessage();
        }


        private string ConvertPathToUrl(string path)
        {
            // Every part of the path that appears in the blog root can be stripped away
            // The blog root path is the html url root
            var subDirectory = path.Replace(_config.BlogRoot, "");
            var lastDirectory = new DirectoryInfo(Path.GetDirectoryName(subDirectory)).Name;
            var fileName = Path.ChangeExtension(Path.GetFileName(path), "html");

            // Above logic fails for files in the blog root
            // because the drive is return rather than a folder
            if(lastDirectory.Contains(':'))
                return $"/{fileName}";


            return $"/{lastDirectory}/{fileName}";
        }

        private void OutputSuccessMessage()
        {
            var original = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            Console.WriteLine("🏁 ✔ 👍 Blog generated 👍 ✔ 🏁");

            Console.ForegroundColor = original;
        }
    }
}
