using blog.generator.contexts;
using blog.generator.processors.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;


namespace blog.generator.processors
{
    public class YamlProcessor : Processor
    {
        readonly IDeserializer _yamlPipeline;


        public YamlProcessor(IDeserializer yamlPipeline)
            => (_yamlPipeline) = (yamlPipeline)
        ;


        public async override Task InvokeAsync(Context context, NextDelegate next)
        {
            var task = Task.Run(() =>
            {
                Console.WriteLine($"Reading front matter from: {context.Article.Markdown.Path}");
                var frontMatter = GetArticleFrontMatter(context.Article.Markdown.Content);

                context.Article.Title = frontMatter.Title;
                context.Article.Slug = frontMatter.Slug;
                context.Article.Tags = frontMatter.Tags;
                context.Article.PostedDate = frontMatter.PostedDate;
                context.Article.Image.Owner = frontMatter.ImageCredit;
                context.Article.Image.Path =  frontMatter.Image;



                FrontMatterModel GetArticleFrontMatter(string article)
                {
                    using var contentIncludingYaml = new StringReader(article);
                    var yamlParser = new Parser(contentIncludingYaml);

                    yamlParser.Consume<StreamStart>();
                    yamlParser.Consume<DocumentStart>();
                    return _yamlPipeline.Deserialize<FrontMatterModel>(yamlParser);
                }
            });

            await task;
            await next(context);
        }
    }
}
