using Blog.Generator.Contexts;
using Blog.Generator.Processors;
using System;
using System.CommandLine.DragonFruit;
using System.IO;
using System.Threading.Tasks;


namespace Blog.Generator
{
    class Program
    {
        //static async Task Main(string buildNumber, string buildSha, string templateRoot, string articlesRoot, string blogRoot)
        static async Task Main(
            string versionNumber,
            string blogRoot,
            string templateRoot,
            string articlesSourceRoot,
            string articlesTargetRoot
        )
        {
            try {
                ValidateInputArgs();
                await Bootstrap().InvokeAsync();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Blog generator failed :(\n{e.Message}");
                Environment.ExitCode = 1;
            }


            void ValidateInputArgs()
            {
                if (String.IsNullOrEmpty(versionNumber))
                    throw new Exception("--version-number is a required arg");

                if ( ! Directory.Exists(templateRoot) )
                    throw new Exception($"Invalid path to site template: {templateRoot}");

                if (! Directory.Exists(articlesSourceRoot) )
                    throw new Exception($"Invalid path to articles: {articlesSourceRoot}");
            }

            App Bootstrap()
            {
                var config = new Config(versionNumber, blogRoot, templateRoot, articlesSourceRoot, articlesTargetRoot);
                var contextBuilder = new ContextBuilder(config);

                var processorPipeline = new ProcessorPipelineBuilder(config)
                    .UseDropExistingSiteProcessor()
                    .UseCloneSiteFromTemplateProcessor()
                    .UseInjectMarkdownArticlesProcessor()
                    .UseYamlProcessor()
                    .UseMarkdownProcessor()
                    .UseIndexPageProcessor()
                    .UseSitemapsProcessor()
                    .Build()
                ;

                return new App(config, contextBuilder, processorPipeline);
            }
        }
    }
}
