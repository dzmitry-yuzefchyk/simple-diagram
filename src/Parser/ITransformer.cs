using SimpleDiagram.Parser.DocumentModel.Ast;

namespace SimpleDiagram.Parser;

public interface ITransformer
{
    string Transform(AstDiagram document, string text);
}
