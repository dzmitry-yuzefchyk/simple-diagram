using SimpleDiagram.Parser.DocumentModel.Ast;

namespace SimpleDiagram.Parser;

public interface IWriter
{
    void Write(AstDiagram document, string text, string outputFile);
}
