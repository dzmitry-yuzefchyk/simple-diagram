using System.Collections.Generic;
using Godot;
using SimpleDiagram.Parser.DocumentModel.Enum;
using SimpleDiagram.Parser.Mermaid.Grammar;

namespace SimpleDiagram.Parser.DocumentModel.Ast;

public class AstDiagram : AstNode
{
    public required MermaidParser.DiagramContext OriginalContext { get; init; }
    public required DiagramType Type { get; init; }
    public required DiagramOrientation Orientation { get; init; }

    private readonly HashSet<AstReference> _references = [];
    private readonly Dictionary<AstNodeId, AstStandaloneNode> _nodes = new();
    private readonly Dictionary<AstNodeId, AstNodePositionComment> _positionComments = new();
    private readonly List<AstWarning> _warnings = [];

    public void AddWarning(AstWarning warning)
    {
        _warnings.Add(warning);
    }

    public void MoveNode(AstNodeId id, Vector2 to)
    {
        if (_nodes.TryGetValue(id, out AstStandaloneNode? node))
        {
            node.Position = to;
        }
    }

    public void AddPositionComment(AstNodePositionComment comment)
    {
        _positionComments.TryAdd(comment.Id, comment);
    }

    public void AddNode(AstStandaloneNode node)
    {
        _nodes.TryAdd(node.Id, node);
    }

    public void AddReference(AstReference reference)
    {
        if (!_nodes.ContainsKey(reference.Parent) && !_nodes.ContainsKey(reference.Child))
        {
            return;
        }

        _references.Add(reference);
    }

    public IEnumerable<AstStandaloneNode> Nodes()
    {
        return _nodes.Values;
    }

    public IEnumerable<AstReference> References()
    {
        return _references;
    }

    public IEnumerable<AstNodePositionComment> Comments()
    {
        return _positionComments.Values;
    }

    public IEnumerable<AstWarning> Warnings()
    {
        return _warnings;
    }
}
