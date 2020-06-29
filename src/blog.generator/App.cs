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


            // marking
            var templateHtml = await File.ReadAllTextAsync(Path.Join(_config.ArticlesBlogRoot, "article.template.html"));
            foreach(var path in Directory.GetFiles(_config.ArticlesBlogRoot, "*.md"))
            {
                var content = File.ReadAllText(path);
                var markupContext = _contextBuilder.BuildMarkupContext(path, content, templateHtml);

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
                ).ToList()
            ;
            var finalisingContext = _contextBuilder.BuildFinaliseContext(htmlFilePaths);
            _processorPipeline.InvokeFinalisePipeline(finalisingContext);




            OutputSuccessMessage();
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
