using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Diagnostics.CodeAnalysis;


namespace Blog.Generator.Db.Models
{
    public class ArticleModel
    {
        [BsonId()]
        public string? Id { get; set; }

        [BsonElement("upVotes")]
        public int UpVotes {get; set; }


        public override string ToString() => $"Article doc: Id: {Id} UpVotes: {UpVotes}";
    }
}
