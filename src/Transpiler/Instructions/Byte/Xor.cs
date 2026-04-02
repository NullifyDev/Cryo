namespace Cryo;

using static Transpiler;

public partial class Instruction
{
    public class Xor : IInstruction
    {
        public byte[] ToABI(ReadOnlySpan<Node> args)
        {
            List<byte> abi = new();
            return abi.ToArray();
        }

        public string ToAsm(ReadOnlySpan<Node> args)
        {
            string asm = args.Length > 1 
                ? $"xor {Handler.GetValue(args[0])}, {Handler.GetValue(args[1])}"
                : $"xor {Handler.GetValue(args[0])}, {Handler.GetValue(args[0])}";
            return asm;
        }
    }
}