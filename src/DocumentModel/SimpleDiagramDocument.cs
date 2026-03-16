using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SimpleDiagram.DocumentModel;

public class SimpleDiagramDocument : IEnumerable<Reference>
{
    private readonly Dictionary<string, Node> _nodes = new();

    public void AddNode(Node node)
    {
        _nodes.TryAdd(node.Id, node);
    }

    public void ConnectNodes(string parentId, string childId, ReferenceType referenceType)
    {
        if (!_nodes.TryGetValue(parentId, out var parentNode))
        {
            return; // TODO: handle that
        }

        if (!_nodes.TryGetValue(childId, out var childNode))
        {
            return; // TODO: handle that
        }

        parentNode.AddChild(childNode, referenceType);
    }

    public IEnumerator<Reference> GetEnumerator()
    {
        // TODO: think about more optimized way to avoid duplicates
        return _nodes
            .SelectMany(x => x.Value.References)
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
