using System;


namespace Blog.Generator.Contexts
{
    public class ScaffoldContext
    {
        public ScaffoldContext()
            : this("", "", "", "", "")
        { }

        public ScaffoldContext(
            string siteRoot,
            string templateSiteRoot,
            string articlesSourceRoot,
            string articlesTargetRoot,
            string versionNumber
        )
            => (SiteRoot, TemplateSiteRoot, ArticlesSourceRoot, ArticlesTargetRoot, VersionNumber)
            =  (siteRoot, templateSiteRoot, articlesSourceRoot, articlesTargetRoot, versionNumber)
        ;


        public string SiteRoot { get; internal set; }
        public string TemplateSiteRoot { get; internal set; }
        public string ArticlesSourceRoot { get; internal set; }
        public string ArticlesTargetRoot { get; internal set; }
        public string VersionNumber { get; internal set; }
    }
}
