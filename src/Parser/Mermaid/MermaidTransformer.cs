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

        foreach (var astNode in document.Nodes.Values)
        {
            if (astNode.Position is null || astNode.Position == Vector2.Zero)
            {
                continue;
            }

            // TODO: decide how to save position in file
            rewriter.InsertBefore(astNode.TokenStartIndex, $"%%POSITION{astNode.Position}%%{Environment.NewLine}");
        }

        return rewriter.GetText();
    }
}
