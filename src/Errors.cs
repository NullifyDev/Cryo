namespace Cryo;

public class Error
{
    public static List<Error> List = new();
    private static Dictionary<ErrorType, string> LUT = new()
    {
        { ErrorType.Syntax_ExpectredIdentifier,   "Expected Identifier: " },
        { ErrorType.Syntax_UnexpectredIdentifier, "Unexpected Identifier: " },

        { ErrorType.Compilation_UnknownObject,    "Found unknown object: " },
        // {  }
    };

    public ErrorType Type;
    public Token     Token;
    public string    Message;

    public Error(ErrorType type, Token token)
    {
        this.Type = type;
        this.Token = token;
        this.Message = "";

    }

    public static void Add(ErrorType type, Token token) => Error.Add(new(type, token));
    public static void Add(Error error)
    {
        error.Message = $"{error.Token.file}({error.Token.line},{error.Token.col}): {Error.LUT[error.Type]} {error.Token.lit}";
        List.Add(error);
    }

    public static void Out()
    {
        foreach(var e in Error.List)
            System.Console.WriteLine(e.Message);
    }
}

public enum ErrorType
{
    // Syntax
    Syntax_ExpectredIdentifier,
    Syntax_UnexpectredIdentifier,

    // Comopilation
    Compilation_UnknownObject,
    Compilation_UnknownRegister,
    Compilation_UnknownLiteralType,

    Compilation_UnexhaustedReturnBranches,

    // Runtime


}