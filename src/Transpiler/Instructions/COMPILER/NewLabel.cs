namespace Cryo;

using static Transpiler;

public partial class Instruction 
{
    public class NewLabel() : IInstruction
    {
        // public NewLabel(){}
        public byte[] ToABI(ReadOnlySpan<Node> args)
        {
            List<byte> abi = new(); 
            return abi.ToArray(); 
        }

        public string ToAsm(ReadOnlySpan<Node> args)
            => $"{Handler.GetValue(args[0])}:" ?? "NO_LABEL_NAME:";
    }
}