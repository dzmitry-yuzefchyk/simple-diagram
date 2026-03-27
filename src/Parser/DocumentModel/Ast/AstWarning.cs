namespace SimpleDiagram.Parser.DocumentModel.Ast;

public record AstWarning(int TokenStartIndex, int TokenEndIndex, string Message);
