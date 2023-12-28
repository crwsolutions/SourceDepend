using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace SourceDepend.Model;

internal sealed class DependencyClassData
{
    public DependencyClassData(INamedTypeSymbol classSymbol, List<ISymbol> taggedMemberSymbols, List<ISymbol>? baseSymbols)
    {
        ClassSymbol = classSymbol;
        TaggedMemberSymbols = taggedMemberSymbols;
        TaggedBaseMemberSymbols = baseSymbols;
    }

    internal INamedTypeSymbol ClassSymbol;

    internal List<ISymbol> TaggedMemberSymbols;

    internal List<ISymbol>? TaggedBaseMemberSymbols;
}
