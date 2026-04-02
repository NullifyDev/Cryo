using Cryo;

public class Instruction;
public interface IInstruction
{
    public byte[] ToABI(ReadOnlySpan<Node> args);
    public string ToAsm(ReadOnlySpan<Node> args);
}