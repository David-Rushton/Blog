using System;


namespace blog.generator.contexts
{
    public class SiteContext
    {
        public SiteContext()
            : this("", "")
        { }

        public SiteContext(string buildNumber, string buildSha)
            => (BuildNumber, BuildSha) = (buildNumber, buildSha)
        ;


        public string BuildNumber { get; set; }
        public string BuildSha { get; set; }
    }
}
