using System;
using System.Diagnostics;
using SimpleDiagram.Parser.DocumentModel.Enum;

namespace SimpleDiagram.Parser.DocumentModel.Ast;

[DebuggerDisplay("{Parent.Id} --> {Child.Id} | {Type}")]
public class AstReference : AstNode
{
    public required AstNodeId Parent { get; init; }
    public required AstNodeId Child { get; init; }
    public ReferenceType Type { get; init; }

    public string Title { get; init; } = string.Empty;

    public override int GetHashCode()
    {
        return HashCode.Combine(Parent, Child);
    }
}
