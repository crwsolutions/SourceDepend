using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace SourceInject;

internal class DependenciesReceiver : ISyntaxContextReceiver
{
    private const string attributeDisplayName = "DependencyAttribute";
    
    public List<IFieldSymbol>? Fields { get; private set; }

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is FieldDeclarationSyntax fieldDeclarationSyntax && fieldDeclarationSyntax.AttributeLists.Count > 0)
        {
            foreach (var variable in fieldDeclarationSyntax.Declaration.Variables)
            {
                if (context.SemanticModel.GetDeclaredSymbol(variable) is IFieldSymbol fieldSymbol)
                {
                    foreach (var att in fieldSymbol.GetAttributes())
                    {
                        if (att.AttributeClass?.ToDisplayString() == attributeDisplayName)
                        {
                            Fields ??= new List<IFieldSymbol>();
                            Fields.Add(fieldSymbol);
                            break; //break attributes foreach
                        }
                    }
                }
            }
        }
    }
}
