using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Svg.Generators
{
    /// <summary>
    /// The SyntaxReceiver is used to filter compiled code. This enable quick and easy way to filter compiled code.
    /// </summary>
    class SyntaxReceiver : ISyntaxReceiver
    {
        /// <summary>
        /// Gets the list of all candidate class.
        /// </summary>
        public List<ClassDeclarationSyntax> CandidateClasses { get; } = new();

        /// <inheritdoc/>
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
            {
                CandidateClasses.Add(classDeclarationSyntax);
            }
        }
    }
}
