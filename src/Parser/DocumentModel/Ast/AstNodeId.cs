namespace SimpleDiagram.Parser.DocumentModel.Ast;

public readonly record struct AstNodeId(string Id)
{
    public override string ToString() => Id;
}
