namespace Cryo;

public partial class Instruction 
{
    public class NewHeader : IInstruction
    {
        public byte[] ToABI(ReadOnlySpan<Node> args)
        {
            List<byte> abi = new(); 
            return abi.ToArray(); 
        }

        public string ToAsm(ReadOnlySpan<Node> args)
            => $".{args[0]}\n";
    }
}