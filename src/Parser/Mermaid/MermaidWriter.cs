using Antlr4.Runtime;
using SimpleDiagram.Parser.DocumentModel.Ast;
using SimpleDiagram.Parser.Mermaid.Grammar;

namespace SimpleDiagram.Parser.Mermaid;

public class MermaidWriter : IWriter
{
    public void Write(AstDiagram document, string text, string outputFile)
    {
        var stream = new AntlrInputStream(text);
        var lexer = new MermaidLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var rewriter = new TokenStreamRewriter(tokens);
    }
}
