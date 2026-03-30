using System.Collections.Generic;
using SimpleDiagram.Parser.DocumentModel.Enum;
using SimpleDiagram.Parser.Mermaid.Grammar;

namespace SimpleDiagram.Parser.DocumentModel.Ast;

public class AstDiagram : AstNode
{
    public required MermaidParser.DiagramContext OriginalContext { get; init; }

    public required DiagramType Type { get; init; }

    public required DiagramOrientation Orientation { get; init; }

    private readonly HashSet<AstReference> _references = [];
    public IReadOnlyCollection<AstReference> References => _references;

    private readonly Dictionary<AstNodeId, AstStandaloneNode> _nodes = new();
    public IReadOnlyDictionary<AstNodeId, AstStandaloneNode> Nodes => _nodes;

    private readonly List<AstWarning> _warnings = [];
    public IEnumerable<AstWarning> Warnings => _warnings;

    public void AddWarning(AstWarning warning)
    {
        _warnings.Add(warning);
    }

    public void AddNode(AstStandaloneNode node)
    {
        _nodes.TryAdd(node.Id, node);
    }

    public void AddReference(AstReference reference)
    {
        if (!Nodes.ContainsKey(reference.Parent) && !Nodes.ContainsKey(reference.Child))
        {
            return;
        }

        _references.Add(reference);
    }
}
