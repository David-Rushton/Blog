using System;
using YamlDotNet.Serialization;


namespace blog.generator.processors.Models
{
    public class FrontMatterModel
    {
        [YamlMember(Alias = "title")]
        public string Title { get; set; }

        [YamlMember(Alias = "slug")]
        public string Slug { get; set; }

        [YamlMember(Alias = "tags")]
        public string[] Tags { get; set; }

        [YamlMember(Alias = "date")]
        public DateTime PostedDate { get; set; }

        [YamlMember(Alias = "image")]
        public string Image { get; set; }

        [YamlMember(Alias = "image-credit")]
        public string ImageCredit { get; set; }
    }
}
