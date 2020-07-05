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
        /// <param name="versionNumber">Version number assigned to this build</param>
        /// <param name="blogRoot">Blog will be generated hee</param>
        /// <param name="templateRoot">Template blog site to copy</param>
        /// <param name="articlesSourceRoot">Location of markdown articles</param>
        /// <param name="articlesTargetRoot">Location to inject articles</param>
        /// <param name="releaseNotesPath">Path to the release notes file</param>
        /// <param name="newBadgeCutoffInDays">The maximum age for articles to be badged as new</param>
        static async Task Main(
            string versionNumber,
            string blogRoot,
            string templateRoot,
            string articlesSourceRoot,
            string articlesTargetRoot,
            string releaseNotesPath,
            int newBadgeCutoffInDays = 10
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
                if(String.IsNullOrEmpty(versionNumber))
                    throw new Exception("--version-number is a required arg");

                if( ! Directory.Exists(templateRoot) )
                    throw new Exception($"Invalid path to site template: {templateRoot}");

                if(! Directory.Exists(articlesSourceRoot) )
                    throw new Exception($"Invalid path to articles: {articlesSourceRoot}");

                if( ! File.Exists(releaseNotesPath) ) {
                    throw new Exception($"Invalid path to release notes: {releaseNotesPath}");
                }
            }

            App Bootstrap()
            {
                var config = new Config(
                    versionNumber, blogRoot, templateRoot, articlesSourceRoot, articlesTargetRoot,
                    releaseNotesPath, newBadgeCutoffInDays
                );
                var contextFactory = new ContextFactory(config);
                var processorPipeline = new ProcessorPipelineBuilder(config)
                    .UseDropExistingSiteProcessor()
                    .UseCloneSiteFromTemplateProcessor()
                    .UseInjectMarkdownArticlesProcessor()
                    .UseYamlProcessor()
                    .UseMarkdownProcessor()
                    .UseArticleNavigationProcessor()
                    .UseArticleSearchProcessor()
                    .UseIndexPageProcessor()
                    .UseSitemapsProcessor()
                    .Build()
                ;

                return new App(config, contextFactory, processorPipeline);
            }
        }
    }
}
