namespace Cryo;

using static Transpiler;

public partial class Instruction 
{
    public class Out : IInstruction
    {
        public byte[] ToABI(ReadOnlySpan<Node> args)
        {
            List<byte> abi = new(); 
            return abi.ToArray(); 
        }

        public string InitAsm(int bit) => $"%include \"linux/{bit}/core/print.asm\"";
        public string ToAsm(ReadOnlySpan<Node> args) => $"mov rsi, {Handler.GetValue(args[0])}\ncall out\n"; 
    }
}

/*
    out:\npush eax\npush ebx\nmov eax, 4\nmov ebx, 1\ncall strlen\nint 80h\npop ebx\npop eax\nret

*/