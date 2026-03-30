using System;
using SimpleDiagram.Parser.DocumentModel.Ast;
using SimpleDiagram.Parser.DocumentModel.Enum;
using SimpleDiagram.Parser.Mermaid.Grammar;

namespace SimpleDiagram.Parser.Mermaid;

public class SimpleDiagramMermaidVisitor : MermaidParserBaseVisitor<AstDiagram>
{
    private AstDiagram? _diagram;

    public override AstDiagram VisitDiagram(MermaidParser.DiagramContext context)
    {
        AstDiagram diagram = new()
        {
            OriginalContext = context,
            Orientation = DiagramOrientation.TopToBottom,
            Type = DiagramType.Flowchart,
            TokenStartIndex = context.Start.TokenIndex,
            TokenStopIndex = context.Stop.TokenIndex,
        };
        _diagram = diagram;

        VisitStatements(context.statements());
        return diagram;
    }

    public override AstDiagram VisitStatements(MermaidParser.StatementsContext context)
    {
        if (_diagram == null)
        {
            throw new InvalidOperationException("Trying to parse input which doesn't start with diagram");
        }

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

        return _diagram;
    }

    public override AstDiagram VisitStandaloneNode(MermaidParser.StandaloneNodeContext context)
    {
        if (_diagram == null)
        {
            throw new InvalidOperationException("Trying to parse input which doesn't start with diagram");
        }

        VisitNodeDefinition(context.nodeDefinition());
        return _diagram;
    }

    public override AstDiagram VisitReference(MermaidParser.ReferenceContext context)
    {
        if (_diagram == null)
        {
            throw new InvalidOperationException("Trying to parse input which doesn't start with diagram");
        }

        var parentContext = context.parentNode;
        var parentId = parentContext.ID().GetText();
        var childContext = context.childNode;
        var childId = childContext.ID().GetText();

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

        VisitNodeDefinition(parentContext);
        VisitNodeDefinition(childContext);

        var reference = new AstReference
        {
            TokenStartIndex = parentContext.Start.TokenIndex,
            TokenStopIndex = childContext.Stop.TokenIndex,
            Parent = new AstNodeId(parentId),
            Child = new AstNodeId(childId),
            Type = linkType,
            Title = ""
        };
        _diagram.AddReference(reference);

        return _diagram;
    }


    public override AstDiagram VisitNodeDefinition(MermaidParser.NodeDefinitionContext context)
    {
        if (_diagram == null)
        {
            throw new InvalidOperationException("Trying to parse input which doesn't start with diagram");
        }

        var id = context.ID().GetText()!;
        var astNode = new AstStandaloneNode
        {
            TokenStartIndex = context.Start.TokenIndex,
            TokenStopIndex = context.Stop.TokenIndex,
            Id = new AstNodeId(id),
            Title = "",
            Position = null,
            Shape = NodeShape.None
        };
        _diagram.AddNode(astNode);

        return _diagram;
    }
}
