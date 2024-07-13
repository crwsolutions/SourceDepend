using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit.Abstractions;

namespace SourceDepend.Tests
{
    internal static class SourceCompiler
    {

        internal static (string, string) GetGeneratedOutput(string source, ITestOutputHelper output)
        {
            var outputCompilation = CreateCompilation(source);
            var trees = outputCompilation.SyntaxTrees.Reverse().Take(2).Reverse().ToList();
            foreach (var tree in trees)
            {
                output.WriteLine(Path.GetFileName(tree.FilePath) + ":");
                output.WriteLine(tree.ToString());
            }
            return (trees.First().ToString(), trees[1].ToString());
        }

        private static Compilation CreateCompilation(string source)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(source);

            var references = new List<MetadataReference>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.IsDynamic && !string.IsNullOrWhiteSpace(assembly.Location))
                {
                    references.Add(MetadataReference.CreateFromFile(assembly.Location));
                }
            }

            var compilation = CSharpCompilation.Create("Foo",
                                                       [syntaxTree],
                                                       references,
                                                       new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var generator = new DependenciesGenerator();

            var driver = CSharpGeneratorDriver.Create(generator);
            driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generateDiagnostics);

            var compileDiagnostics = outputCompilation.GetDiagnostics();

            Assert.False(compileDiagnostics.Any(d => d.Severity == DiagnosticSeverity.Error));
            Assert.False(generateDiagnostics.Any(d => d.Severity == DiagnosticSeverity.Error));

            return outputCompilation;
        }
    }
}
