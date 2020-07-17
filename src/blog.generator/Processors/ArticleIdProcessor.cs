using Blog.Generator.Contexts;
using Blog.Generator.Db;
using Blog.Generator.Processors.Abstractions;
using System;


namespace Blog.Generator.Processors
{
    /// <summary>
    /// Every article has an id and entry in the blog-db.
    /// This class gets/sets that record.
    /// </summary>
    public class ArticleIdProcessor : MarkupProcessor
    {
        readonly ArticleDb _articleDb;


        public ArticleIdProcessor(ArticleDb articleDb) => _articleDb = articleDb;


        public override void Invoke(MarkupContext context)
        {
            var uri = context.Html.Url.RelativeUrl;
            Console.WriteLine($"Searching db for article: {uri}");
            context.Id = _articleDb.GetArticleId(uri);
            Console.WriteLine($"\tFound id: {context.Id}");
        }


        public override string ToString() => "Article Id Processor";
    }
}
