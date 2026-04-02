namespace Cryo;

public class Lexer
{
    private string file, contents;
    private int line, col, curr;

    public Lexer(string file)
    {
        this.file = file;
        this.contents = File.ReadAllText(file);
        this.curr = 0;
        this.line = 1;
        this.col  = 1;
    }

    public IEnumerable<Token> Lex(char? lexItem = null)
    {
        yield return new Token(file, TokenType.SOF, "", 1, 1);
        while(!AtEnd()) 
        {
            char c = lexItem ?? Peek();
            switch(c)
            {
                case ' ': break;
                case '\n':
                    Token t = new Token(file, TokenType.EOL, "\\n", line, col);
                    line++;
                    col=1;
                    yield return t;
                    break;

                case '_':
                    yield return char.IsLetterOrDigit(Next()) 
                        ? new Token(file, TokenType.Underscore, "_", line, col)
                        : scanIdentifier();
                    break;

                case ';':
                    if (Next() == '-') while (Next() != '-' && Peek(1) != ';') ;
                    else               while (Next() != '\n')                  ;
                    break;

                case ':':
                    yield return new Token(file, TokenType.Colon, c.ToString(), line, col);
                    break;

                case '-':
                    yield return Next() == ';' 
                        ? new Token(file, TokenType.CommentMLr, "-;", line, col)
                        : new Token(file, TokenType.Unknown,    "-",  line, col);
                    break;

                case '.':
                    yield return new Token(file, TokenType.Dot, c.ToString(), line, col);
                    break;

                case ',':
                    yield return new Token(file, TokenType.Comma, c.ToString(), line, col);
                    break;

                case '"':
                    yield return scanStringLit();
                    break;

                default:
                    if (char.IsDigit(c))               yield return scanNumberLit();
                    else if (char.IsLetterOrDigit(c))  yield return scanIdentifier();
                    else                               yield return new Token(file, TokenType.Unknown, c.ToString(), line, col);
                    break;
            }
            Next();
        }
        yield return new Token(file, TokenType.EOF, "\\0", line, col);
    }

    private Token scanNumberLit()
    {
        int l = line, c = col;
        string num = Peek().ToString();
        while (char.IsDigit(Peek(1)))
            num += Next();

        return new(file, TokenType.NumberLiteral, num, l, c);
    }

    private Token scanIdentifier()
    {
        int l = line, co = col;
        string token = Peek().ToString();
        while (Peek(1) == '_' || char.IsLetterOrDigit(Peek(1))) token += Next(); 

        return token == "true" || token == "false" 
            ? new(file, TokenType.BoolLiteral, token, l, co) 
            : new(file, TokenType.Identifier,  token, l, co);
    }

    private Token scanStringLit()
    {
        int l = line, c = col;
        string token = Peek().ToString();
        while (Peek(1) != '"') token += Next();

        Next();
        return new(file, TokenType.StringLiteral, token, l, c);
    }

    private bool AtEnd(int ahead = 0) => this.curr + ahead >= this.contents.Length;
    private char Peek (int ahead = 0) => AtEnd(ahead) ? '\0' : this.contents[this.curr+ahead];
    private char Next (int ahead = 1)
    {
        if (ahead < 1) ahead = 1;
        curr+=ahead;
        col+=ahead;
        return Peek();
    }
}