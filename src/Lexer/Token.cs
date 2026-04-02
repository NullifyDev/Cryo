public enum TokenType
{
    Identifier,
    Operator,

    Comma,
    Colon,
    Dot,
    Underscore,

    StringLiteral,
    NumberLiteral,
    BoolLiteral,

    CommentSL,
    CommentMLl,
    CommentMLr,

    EOL,
    EOF,
    SOF, // Start of File

    Unknown,
}

public record Token(string file, TokenType type, string lit, int line, int col)
{
    public override string ToString() => $"{{ file = {file}, type = {type}, lit = \"{lit}\", line = {line}, col = {col} }}"; 
}