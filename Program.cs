using System.Runtime.InteropServices;
using System.Text;

namespace Cryo;

public class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        var handler = new Transpiler.Handler();

        StringBuilder contents = new(),
                      declaration = new();

        // foreach (var n in new Lexer(args[0]).Lex())
        // System.Console.WriteLine(new Library("core", "NullifyDev", "1.0.0", new()).ToJson());
        // LibraryManager.VerifyIntegrity();
        LibraryManager.ScanLibrary();
        // LibraryManager.RepairIntegrity();

        foreach (var n in new Parser(new Lexer(args[0]).Lex()).Parse())
        {
            switch(n)
            {

                case Expression.Label l:
                    contents.Append($"{handler.Handle("newlabel", new[] { 
                        new Expression.Literal(l.name, LiteralType.String, l.file, l.line, l.col)
                    })}\n");
                    break;

                case Expression.Instruction inst:
                    contents.Append($"{handler.Handle((n as Expression.Instruction)!)}\n");
                    break;
            }
            // if (n is Expression.Label) 


            // if (n is Expression.Instruction)
            //     System.Console.WriteLine(handler.Handle((n as Expression.Instruction)!));


            // System.Console.WriteLine(n);
            if (n is Expression.EOF) break;
        }

        System.Console.WriteLine(contents);

        if (Error.List.Count > 0)
            Error.Out();
    }
}