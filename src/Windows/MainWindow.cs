using Godot;
using SimpleDiagram.Parser;
using SimpleDiagram.Parser.DocumentModel.Ast;
using SimpleDiagram.Parser.Mermaid;

namespace SimpleDiagram.Windows;

public partial class MainWindow : Node2D
{
    private DiagramController _controller;
    private IParser _parser;
    private AstDiagram _diagram;
    private GraphEdit _graph;

    public override void _Ready()
    {
        _graph = GetNode<GraphEdit>("%GraphEdit");
        _controller = new DiagramController(new MermaidProcessor(), new MermaidWriter());
        _controller.ReadFile("./Parser/Mermaid/Test.mermaid");
        _diagram = _controller.Diagram!;

        DrawGraph(_graph, _diagram);
    }

    private static void DrawGraph(GraphEdit graph, AstDiagram document)
    {
        foreach (var astNode in document.Nodes.Values)
        {
            var nodeId = $"node-{astNode.Id}";
            var node = new GraphNode();
            var label = new Label();
            node.AddChild(label);
            node.SetSlot(0, true, 0, Colors.White, true, 0, Colors.White);
            node.Name = nodeId;
            node.TooltipText = astNode.Id.ToString();
            node.Title = astNode.Id.ToString();
            graph.AddChild(node);
        }

        foreach (var reference in document.References)
        {
            var parentId = reference.Parent.Id;
            var childId = reference.Child.Id;

            graph.ConnectNode($"node-{parentId}", 0, $"node-{childId}", 0);
        }
    }
}
