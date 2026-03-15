using Godot;

namespace SimpleDiagram.Windows;

public partial class ResizeListener : Node
{
    [Export]
    public Control RootUiNode { get; set; }

    public override void _Ready()
    {
        GetTree().Root.SizeChanged += RootOnSizeChanged;
    }

    public override void _Process(double delta)
    {
    }

    public override void _ExitTree()
    {
        GetTree().Root.SizeChanged -= RootOnSizeChanged;
    }

    private void RootOnSizeChanged()
    {
        if (RootUiNode != null)
        {
            RootUiNode.Size = GetTree().Root.Size;
        }
    }
}
