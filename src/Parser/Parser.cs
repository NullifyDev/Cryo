namespace Cryo;

public partial class Parser
{
    // private IEnumerable<Token> Tokens;
    private IEnumerator<Token> Token;
    private Stack<Node> PastNodes;

    public Parser(IEnumerable<Token> Tokens)
    {
        // this.Tokens = Tokens;
        this.Token = Tokens.GetEnumerator();
        this.PastNodes = new();
    }

    public Node ParseOne(Token? parseItem = null)
    {
        Token t = parseItem ?? Curr();
        Node res = new Expression.Unknown(t, t.file, t.line, t.col);
        // System.Console.WriteLine(t.lit);
        switch(t.type)
        {
            case TokenType.EOF:
                return new Expression.EOF(this.Token.Current.file, this.Token.Current.line+1, 1);

            case TokenType.StringLiteral:
            case TokenType.NumberLiteral:
            case TokenType.BoolLiteral: return new Expression.Literal(t, t.file, t.line, t.col);
            case TokenType.SOF:         return new Expression.SOF(t.file, t.line, t.col);
            case TokenType.EOL:         return new Expression.EOL(t.file, t.line, t.col);

            case TokenType.Identifier:
                if (t.lit.StartsWith("r") 
                &&  Expression.Register.GetRegister(t) != null) {
                    return new Expression.Register(t, t.file, t.line, t.col);
                }
                else if (Expression.Instruction.GetInstructionType(t) != null)
                {
                    List<Node> args = new();
                    while(Next().type != TokenType.EOL && Curr().type != TokenType.EOF) 
                        args.Add(this.ParseOne(Curr()));

                    return new Expression.Instruction(t, args.ToArray(), t.file, t.line, t.col);
                }
                else {
                    Token? n = Next();
                    if (n.type != TokenType.Colon) {
                        Error.Add(ErrorType.Compilation_UnknownObject, t);
                        return res;
                    }
                    return new Expression.Label(t.lit, t.file, t.line, t.col);
                }
                break;

            case TokenType.Dot:
                Token name = Next();
                if (name?.type != TokenType.Identifier)
                    Error.Add(ErrorType.Syntax_ExpectredIdentifier, name!);

                return new Expression.Header(name!.lit, t.file, t.line, t.col);

            default: 
                return res;
        }
    }

    public IEnumerable<Node> Parse()
    {
        while(Next() != null)
            yield return this.ParseOne(Curr());

        yield return new Expression.EOF(this.Token.Current.file, this.Token.Current.line+1, 1);
    }

    private bool IsLiteral(Token t) => IsLiteral(t.type);
    private bool IsLiteral(TokenType t)
        => t == TokenType.StringLiteral 
        || t == TokenType.NumberLiteral 
        || t == TokenType.BoolLiteral;

    private Node NodeReturn(Node node)
    {
        this.PastNodes.Push(node);
        return node;
    }

    // private bool AtEnd(int ahead = 0) => this.curr + ahead >= this.contents.Length;
    // private Token Peek (int ahead = 0) => AtEnd(ahead) ? '\0' : this.Tokens[this.curr+ahead];
    private Token Curr() => this.Token.Current;

    private Node Prev(int past = 1)
    {
        Stack<Node> t = new();
        Node res;
        for(int i = 0; i < past; i++)
            t.Push(this.PastNodes.Pop());

        res = t.Pop();
        t.Push(res);

        while(t.Count > 0)
            this.PastNodes.Push(t.Pop());

        return res;
    }

    private Token? Next(int ahead = 1) 
    {
        return this.Token.MoveNext() ? this.Token.Current : null;
    }
}