using Markdig;
using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


namespace blog.generator.processors
{
    public static class ProcessorPipelineBuilderExtensions
    {
        public static ProcessorPipelineBuilder UseDropExistingSiteProcessor(
            this ProcessorPipelineBuilder pipelineBuilder
        )
        {
            var target = pipelineBuilder._config.BlogRoot;
            pipelineBuilder.RegisterPipelineProcessor(new DropExistingSiteProcessor(target));

            return pipelineBuilder;
        }

        public static ProcessorPipelineBuilder UseCloneSiteFromTemplateProcessor(
            this ProcessorPipelineBuilder pipelineBuilder
        )
        {
            var source = pipelineBuilder._config.TemplateRoot;
            var target = pipelineBuilder._config.BlogRoot;
            pipelineBuilder.RegisterPipelineProcessor(new CloneSiteFromTemplateProcessor(source, target));

            return pipelineBuilder;
        }

        public static ProcessorPipelineBuilder UseInjectMarkdownArticlesProcessor(
            this ProcessorPipelineBuilder pipelineBuilder
        )
        {
            var source = pipelineBuilder._config.BlogRoot;
            var target = Path.Join(pipelineBuilder._config.BlogRoot, "blog.articles");
            pipelineBuilder.RegisterPipelineProcessor(new InjectMarkdownArticlesProcessor(source, target));

            return pipelineBuilder;
        }

        public static ProcessorPipelineBuilder UseYamlProcessor(this ProcessorPipelineBuilder pipelineBuilder)
        {
            var yamlPipeline = new DeserializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build()
            ;

            pipelineBuilder.RegisterPipelineProcessor(new YamlProcessor(yamlPipeline));

            return pipelineBuilder;
        }

        public static ProcessorPipelineBuilder UseMarkdownProcessor(this ProcessorPipelineBuilder pipelineBuilder, string articleTempate)
        {
            var markdownPipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseYamlFrontMatter()
                .Build()
            ;

            pipelineBuilder.RegisterPipelineProcessor(new MarkdownProcessor(markdownPipeline, articleTempate));

            return pipelineBuilder;
        }
    }
}
