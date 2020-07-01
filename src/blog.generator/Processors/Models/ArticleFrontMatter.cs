using System;
using YamlDotNet.Serialization;


namespace Blog.Generator.Processors.Models
{
    public class FrontMatterModel
    {
        // Used to suppress nullable warnings
        public FrontMatterModel()
            => (Title, Slug, Tags,                PostedDate,        Image, ImageCredit)
            =  ("",    "",   new string[] { "" }, DateTime.MinValue, "",    "")
        ;


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
