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
            string articlesTargetRoot,
            string releaseNotesPath,
            string connectionString,
            int newBadgeCutoffInDays
        )
            =>  (VersionNumber, BlogRoot, TemplateRoot, ArticlesSourceRoot, ArticlesTargetRoot, ReleaseNotesPath, ConnectionString, NewBadgeCutoffInDays)
            =   (versionNumber, blogRoot, templateRoot, articlesSourceRoot, articlesTargetRoot, releaseNotesPath, connectionString, newBadgeCutoffInDays)
        ;


        public string VersionNumber { get; internal set; }
        public string BlogRoot { get; internal set; }
        public string TemplateRoot { get; internal set; }
        public string ArticlesSourceRoot { get; internal set; }
        public string ArticlesTargetRoot { get; internal set; }
        public string ReleaseNotesPath { get; internal set; }
        public string ConnectionString { get; internal set; }
        public int NewBadgeCutoffInDays { get; internal set; }


        public override string ToString()
            => String.Format
            (
                "Config:\n\tVersion Number: {0}\n\tBlog Root: {1}\n\tTemplate Root: {2}\n\tArticles Source Root: {3}\n\tArticles Target Root: {4}\n\tRelease Notes Path: {5}\n\tConnection String: xxxxx\n\tNew Badge Cut-off In Days: {6}",
                VersionNumber,
                BlogRoot,
                TemplateRoot,
                ArticlesSourceRoot,
                ArticlesTargetRoot,
                ReleaseNotesPath,
                NewBadgeCutoffInDays.ToString("#,0")
            )
        ;
    }
}
