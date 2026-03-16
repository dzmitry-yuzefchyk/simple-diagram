using System.Diagnostics;

namespace SimpleDiagram.DocumentModel;

[DebuggerDisplay("{Parent.Id} --> {Child.Id} | {ReferenceType}")]
public record Reference(Node Parent, Node Child, ReferenceType ReferenceType);
