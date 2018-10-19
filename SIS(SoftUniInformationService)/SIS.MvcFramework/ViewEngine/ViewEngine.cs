using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SIS.MvcFramework.ViewEngine
{
    public class ViewEngine : IViewEngine
    {
        public string GetHtml<T>(string viewName, string viewCode, T model)
        {
            var viewTypeName = viewName + "View";
            var csharpMethodBody = this.GenereatCSgarpMethodBody(viewCode);

            string viewCodeAsCSharpCode = @"
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SIS.MvcFramework.ViewEngine;
using "+ typeof(T).Namespace+@";

namespace MyAppViews
{
    public class " + viewTypeName + @" : IView<" + typeof(T).FullName.Replace("+", ".") + @">
    {
        public string GetHtml(" + typeof(T).FullName.Replace("+", ".") + @" model)
        {
            StringBuilder html = new StringBuilder();
            var Model = model;
            //TODO View Code here
            " + csharpMethodBody + @"
            return html.ToString().TrimEnd();
        }
    }    
}
";
            //viewCode =>C# code

            var instanceOfViewClass = this.GetInstance(viewCodeAsCSharpCode, "MyAppViews."+ viewTypeName, typeof(T)) as IView<T>;

            var html = instanceOfViewClass.GetHtml(model);
            //C# =>executable object.HetHtml(model)

            return html;
        }

        private object GetInstance(string cSharpCode, string typeName, Type viewModelType)
        {
            //Roslyn
            var tempFileName = Path.GetRandomFileName() + ".dll";

            //create compillable code (Syntax tree):
            var compilation = CSharpCompilation.Create(tempFileName)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("mscorlib")).Location))
                .AddReferences(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("netstandard")).Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView<>).GetTypeInfo().Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IEnumerable<>).GetTypeInfo().Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(Enumerable).GetTypeInfo().Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(viewModelType.Assembly.Location));

            var netStandardReferences = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();
            foreach (var netStandardReference in netStandardReferences)
            {
                compilation = compilation.AddReferences(MetadataReference.CreateFromFile(Assembly.Load(netStandardReference).Location));
            }
            compilation=compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(cSharpCode));

            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> errors = result.Diagnostics
                        .Where(diag => diag.IsWarningAsError || diag.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in errors)
                    {
                        Console.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }

                    return null;
                }

                //dll file will become an assembly . this assembly i sin teh Memeory Straem (ms): 
                ms.Seek(0, SeekOrigin.Begin);
                Assembly assembly = Assembly.Load(ms.ToArray());
                var viewType = assembly.GetType(typeName);

                return Activator.CreateInstance(viewType);
            }
        }

        private string GenereatCSgarpMethodBody(string body)
        {
            var lines = this.GetLines(body);
            var stringBuilder = new StringBuilder();

            foreach (var line in lines)
            {
                var htmlLine = line;
                if (line.Trim().StartsWith("{") ||
                    line.Trim().StartsWith("}")||
                    line.Trim().StartsWith("@for")||
                    line.Trim().StartsWith("@foreach") ||
                    line.Trim().StartsWith("@if") ||
                    line.Trim().StartsWith("else"))
                {
                    //CSharp

                    var firstAtSymbolIndex = line.IndexOf("@", StringComparison.InvariantCulture);
                    stringBuilder.AppendLine(RemoveAt(line, firstAtSymbolIndex));
                }
                else
                {
                    var html = line.Replace("\"", "\\\"");
                    while (htmlLine.Contains("@"))
                    {
                        var specialSymbolIndex = htmlLine.IndexOf("@", StringComparison.InvariantCulture);
                        var endOfCode = new Regex(@"[\s<]+").Match(htmlLine,specialSymbolIndex).Index;
                        string expression = null;
                        if (endOfCode<=0)
                        {
                            expression = htmlLine.Substring(specialSymbolIndex + 1)+" +\"";
                            htmlLine = htmlLine.Substring(0, specialSymbolIndex) + "\"+" + expression + "+\"" ;
                        }
                        else
                        {
                            expression = htmlLine.Substring(specialSymbolIndex + 1, endOfCode - specialSymbolIndex-1);
                             htmlLine = htmlLine.Substring(0, specialSymbolIndex) + "\"+" + expression + "+\"" +htmlLine.Substring(endOfCode);
                        }
                    }

                    stringBuilder.AppendLine($"html.AppendLine(\"{htmlLine}\");");

                }
            }
            return stringBuilder.ToString() ;
        }

        private IEnumerable<string> GetLines(string input)
        {
            var stringReader = new StringReader(input);
            var lines = new List<string>();
            string line = null;
            while ((line = stringReader.ReadLine())!=null)
            {
                lines.Add(line);
            }

            return lines;
        }

        private static string RemoveAt(string input, int index)
        {
            if (index<0)
            {
                return input;
            }
            string result = input.Substring(0, index) + input.Substring(index + 1);

            return result;
        }
    }
}
