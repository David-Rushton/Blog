using System;
using System.IO;


namespace Blog.Generator
{
    public class Config
    {
        public Config(
            string versionNumber,
            string blogRoot,
            string templateRoot,
            string articlesSourceRoot,
            string articlesTargetRoot
        )
            =>  (versionNumber, blogRoot, templateRoot, articlesSourceRoot, articlesTargetRoot)
            =   (VersionNumber, BlogRoot, TemplateRoot, ArticlesSourceRoot, ArticlesTargetRoot)
        ;


        public string VersionNumber { get; internal set; }
        public string BlogRoot { get; internal set; }
        public string TemplateRoot { get; internal set; }
        public string ArticlesSourceRoot { get; internal set; }
        public string ArticlesTargetRoot { get; internal set; }


        public override string ToString()
            => String.Format
            (
                "Config:\n\tVersionNumber{0}\n\tBlogRoot{1}\n\tTemplateRoot{2}\n\tArticlesSourceRoot{3}\n\tArticlesTargetRoot{4}",
                VersionNumber,
                BlogRoot,
                TemplateRoot,
                ArticlesSourceRoot,
                ArticlesTargetRoot
            )
        ;
    }
}
