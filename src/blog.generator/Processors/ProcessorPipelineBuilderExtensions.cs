using Markdig;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


namespace Blog.Generator.Processors
{
    public static class ProcessorPipelineBuilderExtensions
    {
        public static ProcessorPipelineBuilder UseDropExistingSiteProcessor(
            this ProcessorPipelineBuilder pipelineBuilder
        )
        {
            pipelineBuilder.RegisterPipelineProcessor(new DropExistingSiteProcessor());
            return pipelineBuilder;
        }

        public static ProcessorPipelineBuilder UseCloneSiteFromTemplateProcessor(
            this ProcessorPipelineBuilder pipelineBuilder
        )
        {
            pipelineBuilder.RegisterPipelineProcessor(new CloneSiteFromTemplateProcessor());
            return pipelineBuilder;
        }

        public static ProcessorPipelineBuilder UseInjectMarkdownArticlesProcessor(
            this ProcessorPipelineBuilder pipelineBuilder
        )
        {
            pipelineBuilder.RegisterPipelineProcessor(new InjectMarkdownArticlesProcessor());
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

        public static ProcessorPipelineBuilder UseMarkdownProcessor(this ProcessorPipelineBuilder pipelineBuilder)
        {
            var markdownPipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseYamlFrontMatter()
                .Build()
            ;

            pipelineBuilder.RegisterPipelineProcessor(new MarkdownProcessor(markdownPipeline));
            return pipelineBuilder;
        }

        public static ProcessorPipelineBuilder UseSitemapsProcessor(this ProcessorPipelineBuilder pipelineBuilder)
        {
            pipelineBuilder.RegisterPipelineProcessor(new SitemapsProcessor());
            return pipelineBuilder;
        }

        public static ProcessorPipelineBuilder UseIndexPageProcessor(this ProcessorPipelineBuilder pipelineBuilder)
        {
            pipelineBuilder.RegisterPipelineProcessor(new IndexPageProcessor());
            return pipelineBuilder;
        }

        public static ProcessorPipelineBuilder UseArticleNavigationProcessor(
            this ProcessorPipelineBuilder pipelineBuilder
        )
        {
            pipelineBuilder.RegisterPipelineProcessor(new ArticleNavigationProcessor());
            return pipelineBuilder;
        }
    }
}
