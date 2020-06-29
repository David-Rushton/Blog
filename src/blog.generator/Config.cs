using System;
using System.IO;


namespace Blog.Generator
{
    public class Config
    {
        public Config(string buildNumber, string buildSha, string templateRoot, string articlesSourceRoot, string blogRoot)
            =>  (BuildNumber, BuildSha, TemplateRoot, ArticlesSourceRoot, BlogRoot)
            =   (buildNumber, buildSha, templateRoot, articlesSourceRoot, blogRoot)
        ;


        public string BuildNumber { get; set; }
        public string BuildSha { get; set; }
        public string TemplateRoot { get; set; }
        public string ArticlesSourceRoot { get; set; }
        public string ArticlesBlogRoot => Path.Join(BlogRoot, "blog.articles");
        public string BlogRoot  { get; set; }


        public override string ToString()
            => String.Format
            (
                "Config:\n\tBuild Number: {0}\n\tBuild SHA: {1}\n\tTemplate Root: {2}\n\tArticles Sources Root: {3}\n\tBlog Root: {4}\n\tArticles Blog Root: {5}",
                BuildNumber,
                BuildSha,
                TemplateRoot,
                ArticlesSourceRoot,
                BlogRoot,
                ArticlesBlogRoot
            )
        ;
    }
}
