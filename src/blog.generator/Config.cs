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
            => $"Build Number: {BuildNumber} Build SHA: {BuildSha} Template Root: {TemplateRoot}\nArticles Sources Root: {ArticlesSourceRoot}\nBlog Root: {BlogRoot}\nArticles Blog Root: {ArticlesBlogRoot}"
        ;
    }
}
