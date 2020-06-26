namespace blog.generator
{
    public class Config
    {
        public Config(string templateRoot, string articlesRoot, string blogRoot)
            => (TemplateRoot, ArticlesRoot, BlogRoot) = (templateRoot, articlesRoot, blogRoot)
        ;


        public string TemplateRoot { get; set; }
        public string ArticlesRoot { get; set; }
        public string BlogRoot  { get; set; }



        public override string ToString()
            => $"Template Root: {TemplateRoot}\nArticles Root: {ArticlesRoot}\nBlog Root: {BlogRoot}"
        ;
    }
}
