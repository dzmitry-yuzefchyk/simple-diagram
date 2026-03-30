using System.IO;
using System.Linq;
using System.Security.Cryptography;
using SimpleDiagram.Parser.DocumentModel;

namespace SimpleDiagram.Parser;

public class DiagramController(IParser parser, ITransformer transformer)
{
    public SimpleDiagramDocument ReadFile(string file)
    {
        using var stream = File.OpenRead(file);
        var input = File.ReadAllText(file);
        var diagram = parser.Parse(input);

        using var md5 = MD5.Create();
        var hashBytes = md5.ComputeHash(stream);
        var hashStrings = hashBytes.Select(x => x.ToString("X2"));
        var hash = string.Join(string.Empty, hashStrings);

        return new SimpleDiagramDocument { Hash = hash, OriginalText = input, Diagram = diagram };
    }

    public void Write(SimpleDiagramDocument document, string file)
    {
        var newDiagram = transformer.Transform(document.Diagram, document.OriginalText);
        File.WriteAllText(file, newDiagram);
    }
}
