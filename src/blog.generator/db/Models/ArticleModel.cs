using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Diagnostics.CodeAnalysis;


namespace Blog.Generator.Db.Models
{
    /// <summary>
    /// Article entry within the blog-db
    /// </summary>
    [BsonIgnoreExtraElements]
    public class ArticleModel
    {
        public string? Id { get; set; }

        [BsonElement("uri")]
        public string? Uri { get; set; }

        [BsonElement("upVotes")]
        public int upVotes { get; set; }
    }
}
