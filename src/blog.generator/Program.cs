using blog.generator.contexts;
using blog.generator.processors;
using System;
using System.CommandLine.DragonFruit;
using System.IO;
using System.Threading.Tasks;


namespace blog.generator
{
    class Program
    {
        /// <param name="buildNumber">The current build number</param>
        /// <param name="buildSha">The short git SHA, for the committing build</param>
        /// <param name="templateRoot">The site template root folder</param>
        /// <param name="articlesRoot">The articles root folder</param>
        /// <param name="blogRoot">Location to create the blog site</param>
        static async Task Main(string buildNumber, string buildSha, string templateRoot, string articlesRoot, string blogRoot)
        {
            try {
                ValidateInputArgs();
                await (await Bootstrap()).InvokeAsync();
                Console.WriteLine("Blog generated!");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Blog generator failed :(\n{e.Message}");
                Environment.ExitCode = 1;
            }


            void ValidateInputArgs()
            {
                if (String.IsNullOrEmpty(buildNumber))
                    throw new Exception("--build-number is a required arg");

                if (String.IsNullOrEmpty(buildSha))
                    throw new Exception("--build-sha is a required arg");

                if ( ! Directory.Exists(templateRoot) )
                    throw new Exception($"Invalid path to site template: {templateRoot}");

                if (! Directory.Exists(articlesRoot) )
                    throw new Exception($"Invalid path to articles: {articlesRoot}");
            }

            async Task<App> Bootstrap()
            {
                var articleTemplate = File.ReadAllTextAsync(Path.Join(templateRoot, "blog.articles", "article.template.html"));
                var config = new Config(buildNumber, buildSha, templateRoot, articlesRoot, blogRoot);
                var contextBuilder = new ContextBuilder(config);

                var processorPipeline = new ProcessorPipelineBuilder(config)
                    .UseDropExistingSiteProcessor()
                    .UseCloneSiteFromTemplateProcessor()
                    .UseInjectMarkdownArticlesProcessor()
                    .UseYamlProcessor()
                    .UseMarkdownProcessor(await articleTemplate)
                    .Build()
                ;

                return new App(config, contextBuilder, processorPipeline);
            }
        }
    }
}
