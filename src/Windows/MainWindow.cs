using System.Collections.Generic;
using Godot;
using SimpleDiagram.DocumentModel;
using SimpleDiagram.Parser;
using SimpleDiagram.Parser.Mermaid;

namespace SimpleDiagram.Windows;

public partial class MainWindow : Node2D
{
    private IParser _parser;
    private SimpleDiagramDocument _document;
    private GraphEdit _graph;

    public override void _Ready()
    {
        _graph = GetNode<GraphEdit>("%GraphEdit");
        _parser = new MermaidProcessor();
        _document = _parser.Parse("./Parser/Mermaid/Test.mermaid");

        // TODO: this is stupid, tree traversal should be implemented instead to avoid duplicates?
        HashSet<string> addedNodes = new();
        HashSet<(string, string)> addedReferences = new();

        foreach (var reference in _document)
        {
            var parentId = reference.Parent.Id;
            var childId = reference.Child.Id;

            if (!addedNodes.Contains(parentId))
            {
                var parentNode = new GraphNode();
                parentNode.Name = parentId;
                parentNode.TooltipText = parentId;
                parentNode.Title = parentId;
                _graph.AddChild(parentNode);
                addedNodes.Add(parentId);
            }

            if (!addedNodes.Contains(childId))
            {
                var childNode = new GraphNode();
                childNode.Name = childId;
                childNode.TooltipText = childId;
                childNode.Title = childId;
                _graph.AddChild(childNode);
                addedNodes.Add(childId);
            }

            var possibleReference = (parentId, childId);
            if (!addedReferences.Contains(possibleReference))
            {
                _graph.ConnectNode(parentId, 0, childId, 0);
                addedReferences.Add(possibleReference);
            }
        }
    }
}
