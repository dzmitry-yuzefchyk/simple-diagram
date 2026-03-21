using System.Collections.Generic;
using System.Diagnostics;

namespace SimpleDiagram.DocumentModel;

[DebuggerDisplay("{Id} | {_references.Count}")]
public class Node(string id, NodeShape shape, string title = "")
{
    public string Id { get; } = id;

    private readonly List<Reference> _references = new();
    public IEnumerable<Reference> References => _references;

    public void AddChild(Node child, ReferenceType referenceType)
    {
        var reference = new Reference(this, child, referenceType);
        child.AddReference(reference);
        _references.Add(reference);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Node other)
        {
            return false;
        }

        return Id == other.Id;
    }

    public override string ToString()
    {
        return Id;
    }

    private void AddReference(Reference reference)
    {
        _references.Add(reference);
    }
}
