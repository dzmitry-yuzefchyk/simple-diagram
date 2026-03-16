using SimpleDiagram.DocumentModel;

namespace SimpleDiagram.Parser;

public interface IParser
{
    SimpleDiagramDocument Parse(string file);
}
