namespace Cryo;

public partial class Instruction 
{
    public class NOr : IInstruction
    {
        public byte[] ToABI(ReadOnlySpan<Node> args)
        {
            List<byte> abi = new(); 
            return abi.ToArray(); 
        }

        public string ToAsm(ReadOnlySpan<Node> args)
        { 
            string asm = ""; 
            return asm; 
        }
    }
}