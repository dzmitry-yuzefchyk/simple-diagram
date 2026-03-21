using System.Linq;
using SimpleDiagram.DocumentModel;
using SimpleDiagram.Parser.Mermaid.Grammar;

namespace SimpleDiagram.Parser.Mermaid;

public class SimpleDiagramMermaidVisitor : MermaidParserBaseVisitor<SimpleDiagramDocument>
{
    private readonly SimpleDiagramDocument _document = new();

    public override SimpleDiagramDocument VisitDiagram(MermaidParser.DiagramContext context)
    {
        var result = VisitStatements(context.statements());
        return result;
    }

    public override SimpleDiagramDocument VisitStatements(MermaidParser.StatementsContext context)
    {
        foreach (var statementContext in context.statement())
        {
            if (statementContext is MermaidParser.StandaloneNodeContext standaloneNodeContext)
            {
                VisitStandaloneNode(standaloneNodeContext);
            }
            else if (statementContext is MermaidParser.ReferenceContext referenceContext)
            {
                VisitReference(referenceContext);
            }
        }

        return _document;
    }

    public override SimpleDiagramDocument VisitStandaloneNode(MermaidParser.StandaloneNodeContext context)
    {
        var id = context.nodeDefinition().ID().GetText();
        _document.AddNode(new Node(id, NodeShape.RoundEdge));

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

        _document.AddNode(new Node(parentId, NodeShape.None));
        _document.AddNode(new Node(childId, NodeShape.None));
        _document.ConnectNodes(parentId, childId, linkType);

        return _document;
    }
}
