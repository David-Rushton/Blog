using Blog.Generator;
using Blog.Generator.Db.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Libmongocrypt;
using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Linq;


namespace Blog.Generator.Db
{
    public class ArticleDb
    {
        readonly IMongoCollection<ArticleModel> _container;


        public ArticleDb(Config config)
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(config.DbConnectionString));
            settings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = SslProtocols.Tls12
            };

            _container = new MongoClient(settings)
                .GetDatabase(config.DbName)
                .GetCollection<ArticleModel>(config.DbContainer)
            ;
        }


        public string GetArticleId(string uri)
        {
            var filter = Builders<ArticleModel>.Filter.Eq("Uri", uri);
            var map = _container.Find(filter).FirstOrDefault();

            // If the article does not exist in the db then create it
            if(map is null)
            {
                Console.WriteLine($"\tInserting entry into db: {uri}");
                map = new ArticleModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Uri = uri,
                    upVotes = 1
                };

                _container.InsertOne(map);
            }

            if(map?.Id is null)
            {
                throw new Exception($"Cannot locate id in blog-dn for article: {uri}");
            }


            return map.Id;
        }
    }
}
