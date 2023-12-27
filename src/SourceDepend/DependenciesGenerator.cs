using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using SourceDepend.CodeGenerators;
using SourceDepend.Extensions;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SourceDepend;

[Generator]
public partial class DependenciesGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(context => context.AddSource("Dependency.Generated.g.cs", DependencyAttributeCodeGenerator.Generate()));

        var provider = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: static (node, _) => node is ClassDeclarationSyntax,
            transform: static (ctx, _) => ctx.ToDependencyClass())
            .Where(taggedClass => taggedClass is not null);

        var compilation = context.CompilationProvider;

        context.RegisterSourceOutput(provider, Execute);
    }

    private void Execute(SourceProductionContext context, DependencyClassData? classData)
    {
        //if (!Debugger.IsAttached) Debugger.Launch();

        if (classData == null)
        {
            return;
        }

        context.AddSource(
            $"{classData.ClassSymbol.Name}_Dependency.g.cs",
            SourceText.From(DependencyClassCodeGenerator.Generate(
                classData.ClassSymbol,
                classData.TaggedMemberSymbols),
            Encoding.UTF8));
    }
}
