using Blog.Generator.Contexts;
using Blog.Generator.Documents;
using Blog.Generator.Processors.Abstractions;
using Blog.Generator.Processors.Models;
using System;
using System.IO;
using System.Threading.Tasks;
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


        public override void Invoke(MarkupContext context, MarkupDocument markupDocument)
        {
            Console.WriteLine($"Reading front matter form: {markupDocument.MarkdownPath}");

            var frontMatter = GetArticleFrontMatter(context.MarkdownContent);

            markupDocument.Title = frontMatter.Title;
            markupDocument.Slug = frontMatter.Slug;
            markupDocument.Tags = frontMatter.Tags;
            markupDocument.PostedDate = frontMatter.PostedDate;
            markupDocument.Image.Credit = frontMatter.ImageCredit;
            markupDocument.Image.Path = frontMatter.Image;


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
