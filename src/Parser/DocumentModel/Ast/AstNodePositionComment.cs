using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Godot;

namespace SimpleDiagram.Parser.DocumentModel.Ast;

public partial class AstNodePositionComment : AstNode
{
    private const string NodeGroup = "node";
    private const string XPositionGroup = "x";
    private const string YPositionGroup = "y";

    [GeneratedRegex(@$"%%POSITION-(?<{NodeGroup}>.*?)-\((?<{XPositionGroup}>-?\d+),\s*(?<{YPositionGroup}>-?\d+)\)%%")]
    private static partial Regex PositionRegex();

    public AstNodeId Id { get; init; }
    public Vector2 Position { get; init; }

    public static bool TryParse(
        int tokenStartIndex,
        int tokenStopIndex,
        string comment,
        [MaybeNullWhen(false)] out AstNodePositionComment? result
    )
    {
        var match = PositionRegex().Match(comment);
        result = null;
        if (!match.Success)
        {
            return false;
        }

        var node = match.Groups[NodeGroup].Value;
        if (!float.TryParse(match.Groups[XPositionGroup].Value, out var xPosition))
        {
            return false;
        }

        if (!float.TryParse(match.Groups[YPositionGroup].Value, out var yPosition))
        {
            return false;
        }

        var nodeId = new AstNodeId(node);
        var position = new Vector2(xPosition, yPosition);
        result = new AstNodePositionComment
        {
            TokenStartIndex = tokenStartIndex, TokenStopIndex = tokenStopIndex, Id = nodeId, Position = position
        };

        return true;
    }

    public override string ToString() => $"%%POSITION-{Id}-{Position}%%";
}
