using blog.generator.contexts;
using blog.generator.processors;
using Markdig;
using System;
using System.CommandLine.DragonFruit;
using System.IO;
using System.Threading.Tasks;
using YamlDotNet;
using YamlDotNet.Core;
using YamlDotNet.Helpers;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


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
                _ = (await Bootstrap()).InvokeAsync();
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
                var markdownPipeline = new MarkdownPipelineBuilder()
                    .UseAdvancedExtensions()
                    .UseYamlFrontMatter()
                    .Build()
                ;
                var yamlPipeline = new DeserializerBuilder()
                    .WithNamingConvention(HyphenatedNamingConvention.Instance)
                    .Build()
                ;
                var processorPipeline = new ProcessorPipelineBuilder()
                    .RegisterPipelineProcessor(new YamlProcessor(yamlPipeline))
                    .RegisterPipelineProcessor(new MarkdownProcessor(markdownPipeline, await articleTemplate))
                    .Build()
                ;

                return new App(config, contextBuilder, processorPipeline);
            }
        }
    }
}
