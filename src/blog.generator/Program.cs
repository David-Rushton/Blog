using System;
using System.CommandLine.DragonFruit;
using System.IO;
using System.Threading.Tasks;


namespace blog.generator
{
    class Program
    {
        /// <param name="templateRoot">The site template root folder</param>
        /// <param name="articlesRoot">The articles root folder</param>
        /// <param name="blogRoot">Location to create the blog site</param>
        static async Task Main(string templateRoot, string articlesRoot, string blogRoot)
        {
            try {
                ValidateInputArgs();
                await Bootstrap().Invoke();
                Console.WriteLine("Blog generated!");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Blog generator failed :(\n{e.Message}");
                Environment.ExitCode = 1;
            }


            void ValidateInputArgs()
            {
                if ( ! Directory.Exists(templateRoot) )
                    throw new Exception($"Invalid path to site template: {templateRoot}");

                if (! Directory.Exists(articlesRoot) )
                    throw new Exception($"Invalid path to articles: {articlesRoot}");
            }

            App Bootstrap()
            {
                var config = new Config(templateRoot, articlesRoot, blogRoot);
                return new App(config);
            }
        }
    }
}
