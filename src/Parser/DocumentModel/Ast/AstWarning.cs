namespace SimpleDiagram.Parser.DocumentModel.Ast;

public record AstWarning(int StartPosition, int EndPosition, string Message);
