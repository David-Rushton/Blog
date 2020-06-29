using System;


namespace Blog.Generator.Contexts
{
    public class ImageContext
    {
        public ImageContext()
            : this("", "")
        { }

        public ImageContext(string path, string owner)
            => (Path, Owner) = (path, owner)
        ;


        public string Path { get; set; }
        public string Owner { get; set; }

        public string CreditText
            => $"Photo by {Owner} from Pexels";

        public string CreditHtml
            => $"Photo by <strong>{Owner}</strong> from <strong>Pexels</strong>";
    }
}
