using SimpleDiagram.Parser.DocumentModel.Ast;

namespace SimpleDiagram.Parser;

public interface IParser
{
    AstDiagram Parse(string text);
}
