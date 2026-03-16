using System.Linq;
using SimpleDiagram.DocumentModel;
using SimpleDiagram.Parser.Mermaid.Grammar;

namespace SimpleDiagram.Parser.Mermaid;

public class SimpleDiagramMermaidVisitor : MermaidParserBaseVisitor<SimpleDiagramDocument>
{
    private readonly SimpleDiagramDocument _document = new();

    public override SimpleDiagramDocument VisitDiagram(MermaidParser.DiagramContext context)
    {
        var result = VisitNodes(context.nodes());
        return result;
    }

    public override SimpleDiagramDocument VisitNodes(MermaidParser.NodesContext context)
    {
        foreach (var referenceContext in context.reference())
        {
            VisitReference(referenceContext);
        }

        return _document;
    }

    public override SimpleDiagramDocument VisitReference(MermaidParser.ReferenceContext context)
    {
        var parent = context.nodeDefinition().First();
        var parentId = parent.ID().GetText();
        var child = context.nodeDefinition().Last();
        var childId = child.ID().GetText();

        var link = context.link();
        var linkType = ReferenceType.Arrow;
        if (link.LINK_ARROW() != null)
        {
            linkType = ReferenceType.Arrow;
        }
        else if (link.LINK_DOTTED() != null)
        {
            linkType = ReferenceType.Dotted;
        }
        else if (link.LINK_OPEN() != null)
        {
            linkType = ReferenceType.Open;
        }
        else if (link.LINK_THICK() != null)
        {
            linkType = ReferenceType.Thick;
        }

        _document.AddNode(new Node(parentId));
        _document.AddNode(new Node(childId));
        _document.ConnectNodes(parentId, childId, linkType);

        return _document;
    }
}
