namespace blog.generator
{
    public class Config
    {
        public Config(string buildNumber, string buildSha, string templateRoot, string articlesRoot, string blogRoot)
            =>  (BuildNumber, BuildSha, TemplateRoot, ArticlesRoot, BlogRoot)
            =   (buildNumber, buildSha, templateRoot, articlesRoot, blogRoot)
        ;


        public string BuildNumber { get; set; }
        public string BuildSha { get; set; }
        public string TemplateRoot { get; set; }
        public string ArticlesRoot { get; set; }
        public string BlogRoot  { get; set; }


        public override string ToString()
            => $"Template Root: {TemplateRoot}\nArticles Root: {ArticlesRoot}\nBlog Root: {BlogRoot}"
        ;
    }
}
