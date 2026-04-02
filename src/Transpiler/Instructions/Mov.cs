namespace Cryo;

using static Transpiler;

public partial class Instruction 
{
    public class Mov : IInstruction
    {
        public Mov () {}
        public byte[] ToABI(ReadOnlySpan<Node> args)
        {
            List<byte> abi = new(); 
            return abi.ToArray(); 
        }

        public string ToAsm(ReadOnlySpan<Node> args)
        { 
            string asm = $"mov {Handler.GetValue(args[0])}, {Handler.GetValue(args[1])}"; 
            return asm; 
        }
    }
}