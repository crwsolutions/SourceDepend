using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceDepend.Model;
using System.Collections.Generic;

namespace SourceDepend.Extensions
{
    internal static class DependentClassExtensions
    {
        private const string attributeDisplayName = "DependencyAttribute";

        internal static DependencyClassData? ToDependencyClass(this GeneratorSyntaxContext ctx)
        {
            //if (!System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Launch();

            var ClassDeclarationSyntax = (ClassDeclarationSyntax)ctx.Node;
            List<ISymbol>? symbols = null;

            foreach (var member in ClassDeclarationSyntax.Members)
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
                List<ISymbol>? baseSymbols = null;
                if (classSymbol?.BaseType != null)
                {
                    var members = classSymbol.BaseType.GetMembers();
                    {
                        foreach (var member in members)
                        {
                            if (member is IFieldSymbol or
                                IPropertySymbol)
                            {
                                AddSymbolIfTagged(ref baseSymbols, member);
                            }
                        }
                    }
                }

                return classSymbol == null ? null : new DependencyClassData(classSymbol, symbols, baseSymbols);
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
                    return;
                }
            }
        }
    }
}
