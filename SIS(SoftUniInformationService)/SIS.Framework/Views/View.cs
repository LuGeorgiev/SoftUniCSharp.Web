namespace SIS.Framework.Views
{
    using SIS.Framework.ActionResults.Contracts;
    using System.IO;

    public class View : IRenderable
    {
        private readonly string fullyQualifiedTemplateName;

        public View(string fullyQualifiedTemplateName)
        {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
        }

        private string RedadFile(string fullyQualifiedTemplateName)
        {
            if (!File.Exists(fullyQualifiedTemplateName))
            {
                throw new FileNotFoundException($"View does not exist at {fullyQualifiedTemplateName}");
            }

            return File.ReadAllText(this.fullyQualifiedTemplateName);
        }

        public string Render()
        {
            var fullHtml = this.RedadFile(this.fullyQualifiedTemplateName);

            return fullHtml;
        }
    }
}
