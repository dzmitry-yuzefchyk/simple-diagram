using System.Collections.Generic;
using Antlr4.Runtime;
using SimpleDiagram.Parser.DocumentModel.Ast;
using SimpleDiagram.Parser.Mermaid.Grammar;

namespace SimpleDiagram.Parser.Mermaid;

public class MermaidProcessor : IParser
{
    public AstDiagram Parse(string text)
    {
        var stream = new AntlrInputStream(text);
        var lexer = new MermaidLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new MermaidParser(tokens);
        var visitor = new SimpleDiagramMermaidVisitor();

        var diagram = parser.diagram();
        var result = visitor.Visit(diagram);

        ParsePositions(result, tokens.GetTokens());

        return result;
    }

    private void ParsePositions(AstDiagram diagram, IList<IToken> tokens)
    {
        foreach (var token in tokens)
        {
            if (token.Channel != MermaidLexer.POSITIONS_CHANNEL)
            {
                continue;
            }

            var comment = token.Text;
            var created = AstNodePositionComment.TryParse(
                token.TokenIndex,
                token.TokenIndex,
                comment,
                out var commentNode
            );

            if (!created)
            {
                continue;
            }

            diagram.AddPositionComment(commentNode!);
            if (diagram.Nodes.TryGetValue(commentNode!.Id, out AstStandaloneNode? diagramNode))
            {
                diagramNode.Position = commentNode.Position;
            }
        }
    }
}
