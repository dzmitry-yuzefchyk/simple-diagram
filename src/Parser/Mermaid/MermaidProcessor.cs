using Antlr4.Runtime;
using SimpleDiagram.Parser.Mermaid.Grammar;

namespace SimpleDiagram.Parser.Mermaid;

public class MermaidProcessor : IParser
{
    public object Parse()
    {
        var input = "";
        var stream = new AntlrInputStream(input);
        var lexer = new MermaidLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new MermaidParser(tokens);
        var visitor = new SimpleDiagramMermaidVisitor();

        var diagram = parser.diagram();
        return visitor.Visit(diagram);
    }
}
