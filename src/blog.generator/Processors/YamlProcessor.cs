using Blog.Generator.Contexts;
using Blog.Generator.Processors.Abstractions;
using Blog.Generator.Processors.Models;
using System;
using System.IO;
using System.Linq;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;


namespace Blog.Generator.Processors
{
    public class YamlProcessor : MarkupProcessor
    {
        readonly IDeserializer _yamlPipeline;


        public YamlProcessor(IDeserializer yamlPipeline)
            => (_yamlPipeline) = (yamlPipeline)
        ;


        public override void Invoke(MarkupContext context)
        {
            Console.WriteLine($"Reading front matter form: {context.Markdown.Path}");

            var frontMatter = GetArticleFrontMatter(context.Markdown.Content);

            context.Title = frontMatter.Title;
            context.Slug = frontMatter.Slug;
            context.Tags = frontMatter.Tags.Select(t => context.NewTagItem(t)).ToList();
            context.PostedDate = frontMatter.PostedDate;
            context.Image.Credit = frontMatter.ImageCredit;
            context.Image.Provider = frontMatter.ImageProvider;
            context.Image.Url = frontMatter.Image;


            FrontMatterModel GetArticleFrontMatter(string article)
            {
                using var contentIncludingYaml = new StringReader(article);
                var yamlParser = new Parser(contentIncludingYaml);

                yamlParser.Consume<StreamStart>();
                yamlParser.Consume<DocumentStart>();
                return _yamlPipeline.Deserialize<FrontMatterModel>(yamlParser);
            }
        }

        public override string ToString()
            => "Yaml Processor"
        ;
    }
}
