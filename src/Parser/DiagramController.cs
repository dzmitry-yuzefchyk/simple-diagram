using System.IO;
using SimpleDiagram.Parser.DocumentModel.Ast;

namespace SimpleDiagram.Parser;

public class DiagramController(IParser parser, IWriter writer)
{
    public AstDiagram? Diagram { get; private set; }

    public void ReadFile(string file)
    {
        var input = File.ReadAllText(file);
        Diagram = parser.Parse(input);
    }

    public void Write(AstDiagram document, string file)
    {
        var text = "";
        writer.Write(document, text, file);
    }
}
