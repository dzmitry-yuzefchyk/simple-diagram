using System.Collections.Generic;
using Antlr4.Runtime;
using Godot;
using SimpleDiagram.Parser.DocumentModel.Ast;
using SimpleDiagram.Parser.Mermaid.Grammar;
using Environment = System.Environment;

namespace SimpleDiagram.Parser.Mermaid;

public class MermaidTransformer : ITransformer
{
    public string Transform(AstDiagram document, string text)
    {
        var stream = new AntlrInputStream(text);
        var lexer = new MermaidLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        tokens.Fill();
        var rewriter = new TokenStreamRewriter(tokens);

        var processedNodes = new HashSet<AstNodeId>();
        OverrideExistingComments(rewriter, processedNodes, document);
        AppendMissingComments(rewriter, processedNodes, document);

        return rewriter.GetText();
    }

    private void OverrideExistingComments(
        TokenStreamRewriter rewriter,
        HashSet<AstNodeId> processedNodes,
        AstDiagram document
    )
    {
        foreach (var (_, comment) in document.PositionComments)
        {
            if (!processedNodes.Add(comment.Id))
            {
                continue;
            }

            rewriter.Replace(comment.TokenStartIndex, comment.TokenStopIndex, comment.ToString());
        }
    }

    private void AppendMissingComments(
        TokenStreamRewriter rewriter,
        HashSet<AstNodeId> processedNodes,
        AstDiagram document
    )
    {
        foreach (var (_, node) in document.Nodes)
        {
            if (!processedNodes.Add(node.Id))
            {
                continue;
            }

            if (node.Position is null || node.Position == Vector2.Zero)
            {
                continue;
            }

            var comment = new AstNodePositionComment
            {
                TokenStartIndex = 0, TokenStopIndex = 0, Id = node.Id, Position = node.Position.Value
            };
            rewriter.InsertAfter(document.TokenStopIndex, $"{Environment.NewLine}{comment}");
        }
    }
}
