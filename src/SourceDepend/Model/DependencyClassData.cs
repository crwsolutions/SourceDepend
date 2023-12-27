using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace SourceDepend;

public partial class DependenciesGenerator
{
    internal sealed class DependencyClassData
    {
        public DependencyClassData(INamedTypeSymbol classSymbol, List<ISymbol> taggedMemberSymbols)
        {
            ClassSymbol = classSymbol;
            TaggedMemberSymbols = taggedMemberSymbols;
        }

        internal INamedTypeSymbol ClassSymbol;

        internal List<ISymbol> TaggedMemberSymbols;
    }
}
