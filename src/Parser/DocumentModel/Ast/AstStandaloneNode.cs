using Godot;
using SimpleDiagram.Parser.DocumentModel.Enum;

namespace SimpleDiagram.Parser.DocumentModel.Ast;

public class AstStandaloneNode : AstNode
{
    public required AstNodeId Id { get; init; }

    public NodeShape Shape { get; init; }
    public string Title { get; set; } = string.Empty;
    public Vector2? Position { get; set; }
}
