using System;
using System.IO;


namespace blog.generator
{
    public static class FileSystemHelper
    {
        public static void DeepCopyDirectory(string source, string target)
            => DeepCopyDirectory(new DirectoryInfo(source), new DirectoryInfo(target))
        ;

        public static void DeepCopyDirectory(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                DeepCopyDirectory(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
