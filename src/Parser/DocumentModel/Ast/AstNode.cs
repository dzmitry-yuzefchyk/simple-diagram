namespace SimpleDiagram.Parser.DocumentModel.Ast;

public abstract class AstNode
{
    public required int TokenStartIndex { get; init; }
    public required int TokenStopIndex { get; init; }
}