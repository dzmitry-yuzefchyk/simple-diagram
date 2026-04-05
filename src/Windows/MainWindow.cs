using System.Linq;
using Godot;
using SimpleDiagram.Parser;
using SimpleDiagram.Parser.DocumentModel;
using SimpleDiagram.Parser.DocumentModel.Ast;
using SimpleDiagram.Parser.Mermaid;

namespace SimpleDiagram.Windows;

public partial class MainWindow : Node2D
{
    // TODO: figure that out, it's a hack for node connections to properly work on each subsequent file open
    // seems like node name is stored as some unique key which conflicts with connections getting cleared up
    private int _fileCount;
    private DiagramController _controller;
    private SimpleDiagramDocument _document;

    private string _file = string.Empty;

    [Export]
    private GraphEdit _graph = null!;

    [Export]
    private Button _openFileButton = null!;

    [Export]
    private FileDialog _dialog = null!;

    public override void _Ready()
    {
        CloseDialog();
        _controller = new DiagramController(new MermaidProcessor(), new MermaidTransformer());
    }

    private void DrawGraph(GraphEdit graph, AstDiagram document)
    {
        var existingGraphNodes = graph
            .GetChildren()
            .Where(x => x is GraphNode);

        foreach (var existingGraphNode in existingGraphNodes)
        {
            existingGraphNode.QueueFree();
        }
        graph.ClearConnections();

        foreach (var astNode in document.Nodes.Values)
        {
            var nodeId = $"{_fileCount}-node-{astNode.Id}";
            var node = new GraphNode();
            var label = new Label();
            node.AddChild(label);
            node.SetSlot(0, true, 0, Colors.White, true, 0, Colors.White);
            node.Name = nodeId;
            node.TooltipText = astNode.Id.ToString();
            node.Title = astNode.Id.ToString();

            if (astNode.Position is not null)
            {
                node.PositionOffset = astNode.Position.Value;
            }

            node.Dragged += (from, to) => GraphNodeMoved(astNode.Id, from, to);
            graph.AddChild(node);
        }

        foreach (var reference in document.References)
        {
            var parentId = reference.Parent.Id;
            var childId = reference.Child.Id;

            graph.ConnectNode($"{_fileCount}-node-{parentId}", 0, $"{_fileCount}-node-{childId}", 0);
        }
    }

    private void OnOpenFilePressed()
    {
        if (_dialog.Visible)
        {
            return;
        }

        _openFileButton.Disabled = true;
        _dialog.Visible = true;
    }

    private void GraphNodeMoved(AstNodeId id, Vector2 from, Vector2 to)
    {
        _document.Diagram.Nodes[id].Position = to;
    }

    private void OnFileNotSelected()
    {
        CloseDialog();
    }

    private void OnSaveFilePressed()
    {
        if (string.IsNullOrEmpty(_file))
        {
            return;
        }

        _controller.Write(_document, _file);
        OnFileSelected(_file);
    }

    private void OnFileSelected(string file)
    {
        _file = file;
        _document = _controller.ReadFile(file);
        DrawGraph(_graph, _document.Diagram);

        _fileCount += 1;
        CloseDialog();
    }

    private void CloseDialog()
    {
        _dialog.Visible = false;
        _openFileButton.Disabled = false;
    }
}
