using Blog.Generator.Contexts;
using Blog.Generator.Documents;
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
        readonly MarkupDocuments _markupDocuments;
        readonly ContextBuilder _contextBuilder;
        readonly ProcessorPipeline _processorPipeline;


        public App(Config config, MarkupDocuments markupDocuments, ContextBuilder contextBuilder, ProcessorPipeline processorPipeline)
        {
            _config = config;
            _markupDocuments = markupDocuments;
            _contextBuilder = contextBuilder;
            _processorPipeline = processorPipeline;
        }


        public async Task InvokeAsync()
        {
            Console.WriteLine("Generating blog...");
            Console.WriteLine($"Config:\n{_config}");


            // scaffolding
            var siteContext = _contextBuilder.BuildSiteContext();
            _processorPipeline.InvokeScaffoldPipeline(siteContext);


            // marking
            var templateHtml = await File.ReadAllTextAsync(Path.Join(_config.ArticlesBlogRoot, "article.template.html"));
            Parallel.ForEach(Directory.GetFiles(_config.ArticlesBlogRoot), path =>
                {
                    var content = File.ReadAllText(path);
                    var markupContext = _contextBuilder.BuildMarkupContext(path, content, templateHtml);
                    var markupDocument = _markupDocuments.New(path);

                    _processorPipeline.InvokeMarkupPipeline(markupContext, markupDocument);
                })
            ;



            // finalising



            Console.WriteLine("Blog generated!");
            return;
        }
    }
}
