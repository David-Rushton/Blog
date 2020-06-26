using System;
using System.IO;
using System.Threading.Tasks;


namespace blog.generator
{
    public class App
    {
        readonly Config _config;


        public App(Config config)
            => (_config) = (config)
        ;


        public async Task Invoke()
        {
            Console.WriteLine($"Config:\n{_config}");

            DeleteBlogSiteIfExists();
            CloneBlogSiteFromTemplate();

            // Copy template
            // Copy articles
            // Process articles

                // pub/sub | visitor | events | pass in a subscribers?

                // build index
                // build sitemap
                // build blog list pate
                // rewrite .md links
                // build articles





            return;
        }


        private void DeleteBlogSiteIfExists()
        {
            if (Directory.Exists(_config.BlogRoot))
            {
                Console.Write("Deleting existing blog site");
                Directory.Delete(_config.BlogRoot, true);
            }
        }

        private void CloneBlogSiteFromTemplate()
            => FileSystemHelper.DeepCopyDirectory(_config.TemplateRoot, _config.BlogRoot)
        ;
    }
}
