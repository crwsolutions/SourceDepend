using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace SourceDepend;

internal class DependenciesReceiver : ISyntaxContextReceiver
{
    private const string attributeDisplayName = "DependencyAttribute";
    
    public List<ISymbol>? Symbols { get; private set; }

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is FieldDeclarationSyntax fieldDeclarationSyntax && fieldDeclarationSyntax.AttributeLists.Count > 0)
        {
            //System.Diagnostics.Debugger.Launch();

            foreach (var variable in fieldDeclarationSyntax.Declaration.Variables)
            {
                if (context.SemanticModel.GetDeclaredSymbol(variable) is IFieldSymbol fieldSymbol)
                {
                    foreach (var att in fieldSymbol.GetAttributes())
                    {
                        if (att.AttributeClass?.ToDisplayString() == attributeDisplayName)
                        {
                            Symbols ??= new List<ISymbol>();
                            Symbols.Add(fieldSymbol);
                            break; //break attributes foreach
                        }
                    }
                }
            }
        }

        if (context.Node is PropertyDeclarationSyntax propertyDeclarationSyntax && propertyDeclarationSyntax.AttributeLists.Count > 0)
        {
            if (context.SemanticModel.GetDeclaredSymbol(propertyDeclarationSyntax) is IPropertySymbol propertySymbol)
            {
                foreach (var att in propertySymbol.GetAttributes())
                {
                    if (att.AttributeClass?.ToDisplayString() == attributeDisplayName)
                    {
                        Symbols ??= new List<ISymbol>();
                        Symbols.Add(propertySymbol);
                        break; //break attributes foreach
                    }
                }
            }
        }
    }
}
