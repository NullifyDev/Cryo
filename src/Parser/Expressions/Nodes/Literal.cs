namespace Cryo;

public partial class Expression
{
    public record Literal : Node
    {
        public object? Value;
        public LiteralType Type;

        public LiteralType? GetLiteralType(Token token)
            => token.type switch
                {
                    TokenType.StringLiteral => LiteralType.String,
                    TokenType.NumberLiteral => LiteralType.Number,
                    TokenType.BoolLiteral   => LiteralType.Bool,
                    _                       => null
                };

        public Literal(object value, LiteralType lt, string file, int line, int col) : base(file, line, col)
        {
            this.Value = value;
            this.Type = lt;
        }

        public Literal(Token token, string file, int line, int col) : base(file, line, col)
        {
            LiteralType? l = GetLiteralType(token);
            if (l == null)
            {
                Error.Add(ErrorType.Compilation_UnknownLiteralType, token);
                return;
            }
            this.Type = (LiteralType)l;
            switch(this.Type)
            {
                case LiteralType.String: this.Value = token.lit; break;
                case LiteralType.Number:
                    int.TryParse(token.lit, out var n);
                    this.Value = n;
                    break;

                case LiteralType.Bool:
                    bool.TryParse(token.lit, out var b);
                    this.Value = b;
                    break;

                default: 
                    this.Value =  null; 
                    break;
            };
        }
    }
}

public enum LiteralType
{
    String,
    Number,
    Bool,
}