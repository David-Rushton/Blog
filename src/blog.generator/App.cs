using blog.generator.contexts;
using blog.generator.processors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace blog.generator
{
    public class App
    {
        readonly Config _config;
        readonly ContextBuilder _contextBuilder;
        readonly ProcessorPipeline _processorPipeline;
        readonly string _blogArticlePath;


        public App(Config config, ContextBuilder contextBuilder, ProcessorPipeline processorPipeline)
        {
            _config = config;
            _contextBuilder = contextBuilder;
            _processorPipeline = processorPipeline;
            _blogArticlePath = Path.Join(_config.BlogRoot, "blog.articles");
        }


        public async Task InvokeAsync()
        {
            Console.WriteLine($"Config:\n{_config}");

            // Rebuild the site from the template and raw content
            DeleteBlogSiteIfExists();
            CloneBlogSiteFromTemplate();
            InjectMarkdownArticlesIntoBlogSite();


            // The processor pipeline converts the raw content into the finished article
            List<Task> contextTasks = new List<Task>
            (
                Directory
                    .GetFiles(_blogArticlePath, "*.md")
                    .Select(async path =>
                        {
                            var content = File.ReadAllTextAsync(path);
                            var context = _contextBuilder.Build(path, await content);
                            return _processorPipeline.InvokePipelineAsync(context);
                        })
                    .ToList()
            );

            Task t = Task.WhenAll(contextTasks);
            await t;
            return;
        }


        private void DeleteBlogSiteIfExists()
        {
            if (Directory.Exists(_config.BlogRoot))
            {
                Console.Write("Deleting existing blog site");
                Directory.Delete(_config.BlogRoot, true);
            }
        }

        private void CloneBlogSiteFromTemplate()
            => FileSystemHelper.DeepCopyDirectory(_config.TemplateRoot, _config.BlogRoot)
        ;

        private void InjectMarkdownArticlesIntoBlogSite()
            => FileSystemHelper.DeepCopyDirectory(_config.ArticlesRoot, _blogArticlePath)
        ;
    }
}
