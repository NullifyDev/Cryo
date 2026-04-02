namespace Cryo;

public partial class Expression 
{
 
    public record Instruction : Node
    {
        public InstructionType Type;
        public Node[] Arguments;
    
        public Instruction(Token token, Node[] args, string file, int line, int col) : base(file, line, col)
        {
            this.Arguments = args;
            InstructionType? t = GetInstructionType(token);
            if (t == null)
            {
                Error.Add(ErrorType.Compilation_UnknownObject, token);
                return;
            }
            this.Type = (InstructionType)t!;
        }

        public override string ToString()
            => $"Instruction {{ file = {this.file}, line = {this.line}, col = {this.col}, Type = {this.Type}, Arguments = [ {string.Join(", ", this.Arguments)} ] }}";

        public static bool IsInstruction(Token token)
            => GetInstructionType(token) != null;

        public static InstructionType? GetInstructionType(Token token)
        => token.lit.ToLower() switch {
            "and"  => InstructionType.And,
            "or"   => InstructionType.Or,
            "xor"  => InstructionType.Xor,
            "not"  => InstructionType.Not,
            "nand" => InstructionType.NAnd,
            "nor"  => InstructionType.NOr,
            "nxor" => InstructionType.NXor,
            "add"  => InstructionType.Add,
            "sub"  => InstructionType.Sub,
            "mov"  => InstructionType.Mov,
            "out"  => InstructionType.Out,
            "open" => InstructionType.Open,
            _      => null
        };
    }
}

public enum InstructionType : byte
{
    Open,

    And,
    Or,
    Xor,
    Not,
    NAnd,
    NOr,
    NXor,

    Add,
    Sub,
    Mov,
    
    Out,
}