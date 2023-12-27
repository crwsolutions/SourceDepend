using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using static SourceDepend.DependenciesGenerator;

namespace SourceDepend.Extensions
{
    internal static class DependentClassExtensions
    {
        private const string attributeDisplayName = "DependencyAttribute";

        internal static DependencyClassData? ToDependencyClass(this GeneratorSyntaxContext ctx)
        {
            var ClassDeclarationSyntax = (ClassDeclarationSyntax)ctx.Node;

            List<ISymbol>? symbols = null;
            var members = ClassDeclarationSyntax.Members;

            foreach (var member in members)
            {
                if (member.AttributeLists.Any() == false)
                {
                    continue;
                }

                if (member is FieldDeclarationSyntax fieldSyntax)
                {
                    foreach (var variable in fieldSyntax.Declaration.Variables)
                    {
                        if (ctx.SemanticModel.GetDeclaredSymbol(variable) is ISymbol symbol)
                        {
                            AddSymbolIfTagged(ref symbols, symbol);
                        }
                    }
                }
                else if (member.IsKind(SyntaxKind.PropertyDeclaration))
                {
                    if (ctx.SemanticModel.GetDeclaredSymbol(member) is ISymbol symbol)
                    {
                        AddSymbolIfTagged(ref symbols, symbol);
                    }
                }
            }

            if (symbols != null)
            {
                var classSymbol = ctx.SemanticModel.GetDeclaredSymbol(ClassDeclarationSyntax);
                return classSymbol == null ? null : new DependencyClassData(classSymbol, symbols);
            }

            return null;
        }

        private static void AddSymbolIfTagged(ref List<ISymbol>? symbols, ISymbol fieldSymbol)
        {
            foreach (var att in fieldSymbol.GetAttributes())
            {
                if (att.AttributeClass?.ToDisplayString() == attributeDisplayName)
                {
                    symbols ??= [];
                    symbols.Add(fieldSymbol);
                    break; //break attributes foreach
                }
            }
        }
    }
}
