using Blog.Generator;
using Blog.Generator.Db.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Libmongocrypt;
using System;
using System.Security.Authentication;
using System.Linq;


namespace Blog.Generator.Db
{
    public class Articles
    {
        readonly MongoClient _mongoClient;
        readonly IMongoDatabase _db;
        readonly IMongoCollection<ArticleModel> _container;


        public Articles(Config config)
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(config.DbConnectionString));
            settings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = SslProtocols.Tls12
            };

            _mongoClient = new MongoClient(settings);
            _db = _mongoClient.GetDatabase(config.DbName);
            _container = _db.GetCollection<ArticleModel>(config.DbContainer);
        }


        public int GetUpvoteCount(string id)
        {
            var filter = Builders<ArticleModel>.Filter.Eq("Id", id);
            var article = _container.Find(filter).FirstOrDefault();

            return article?.UpVotes ?? 1;
        }
    }
}
