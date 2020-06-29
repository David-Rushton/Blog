using System;


namespace Blog.Generator.Contexts
{
    public class ScaffoldContext
    {
        public ScaffoldContext()
            : this("", "", "", "", "", "")
        { }

        public ScaffoldContext(string siteRoot, string templateSiteRoot, string articleSource, string articleTarget, string buildNumber, string buildSha)
            => (SiteRoot, TemplateSiteRoot, ArticleSource, ArticleTarget, BuildNumber, BuildSha)
            =  (siteRoot, templateSiteRoot, ArticleSource, ArticleTarget, buildNumber, buildSha)
        ;


        public string SiteRoot { get; internal set; }
        public string TemplateSiteRoot { get; internal set; }
        public string ArticleSource { get; internal set; }
        public string ArticleTarget { get; internal set; }
        public string BuildNumber { get; internal set; }
        public string BuildSha { get; internal set; }
    }
}
