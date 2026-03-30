using SimpleDiagram.Parser.DocumentModel.Ast;

namespace SimpleDiagram.Parser.DocumentModel;

public class SimpleDiagramDocument
{
    public required string Hash { get; init; }
    public required string OriginalText { get; init; }
    public required AstDiagram Diagram { get; init; }
}
