namespace SIS.Framework.Views
{
    using SIS.Framework.ActionResults.Contracts;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class View : IRenderable
    {
        private readonly string fullyQualifiedTemplateName;
        private readonly IDictionary<string, object> viewDate;

        public View(string fullyQualifiedTemplateName, 
            IDictionary<string, object> viewDate)
        {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
            this.viewDate = viewDate;
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
            string renderHtml = this.RenderHtml(fullHtml);

            return fullHtml;
        }

        private string RenderHtml(string fullHtml)
        {
            var renderedHtml = fullHtml;

            if (this.viewDate.Any())
            {
                foreach (var parameter in this.viewDate)
                {
                    string dynamicPlaceholder = $"{{{{{{{parameter.Key}}}}}}}";
                    if (renderedHtml.Contains(dynamicPlaceholder))
                    {
                        renderedHtml = renderedHtml
                            .Replace(dynamicPlaceholder, parameter.Value.ToString());
                    }
                }
            }

            return renderedHtml;
        }
    }
}
