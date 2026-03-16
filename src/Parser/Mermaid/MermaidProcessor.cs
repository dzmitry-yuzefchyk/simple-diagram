using System.IO;
using Antlr4.Runtime;
using SimpleDiagram.DocumentModel;
using SimpleDiagram.Parser.Mermaid.Grammar;

namespace SimpleDiagram.Parser.Mermaid;

public class MermaidProcessor : IParser
{
    public SimpleDiagramDocument Parse(string file)
    {
        var input = File.ReadAllText(file);
        var stream = new AntlrInputStream(input);
        var lexer = new MermaidLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new MermaidParser(tokens);
        var visitor = new SimpleDiagramMermaidVisitor();

        var diagram = parser.diagram();
        SimpleDiagramDocument result = visitor.Visit(diagram);

        return result;
    }
}
