namespace Cryo;

public partial class Expression 
{
    public record Register : Node
    {
        public RegisterType Type;
    
        public Register(Token token, string file, int line, int col) : base(file, line, col)
        {
            RegisterType? rt = GetRegister(token);
            if (rt == null)
            {
                Error.Add(ErrorType.Compilation_UnknownRegister, token);
                return;
            }
            this.Type = (RegisterType)rt;
        }

        public static RegisterType? GetRegister(Token token)
            => token.lit switch {
            // 8 Bit
            "r0b" => RegisterType.r8b, 
            "r1b" => RegisterType.r9b,
            "r2b" => RegisterType.r10b,
            "r3b" => RegisterType.r11b,
            "r4b" => RegisterType.r12b,
            "r5b" => RegisterType.r13b,
            "r6b" => RegisterType.r14b,
            "r7b" => RegisterType.r15b,

            // 16 Bit
            "r0w" => RegisterType.r8w,
            "r1w" => RegisterType.r9w,
            "r2w" => RegisterType.r10w,
            "r3w" => RegisterType.r11w,
            "r4w" => RegisterType.r12w,
            "r5w" => RegisterType.r13w,
            "r6w" => RegisterType.r14w,
            "r7w" => RegisterType.r15w,

            // 32 Bit
            "r0d" => RegisterType.r8d,
            "r1d" => RegisterType.r9d,
            "r2d" => RegisterType.r10d,
            "r3d" => RegisterType.r11d,
            "r4d" => RegisterType.r12d,
            "r5d" => RegisterType.r13d,
            "r6d" => RegisterType.r14d,
            "r7d" => RegisterType.r15d,

            // 64 Bit
            "r0"  => RegisterType.r8,
            "r1"  => RegisterType.r9,
            "r2"  => RegisterType.r10,
            "r3"  => RegisterType.r11,
            "r4"  => RegisterType.r12,
            "r5"  => RegisterType.r13,
            "r6"  => RegisterType.r14,
            "r7"  => RegisterType.r15,
            _ => null
        };
    }
}

public enum RegisterType
{
    r8b,
    r9b,
    r10b,
    r11b,
    r12b,
    r13b,
    r14b,
    r15b,
    r8w,
    r9w,
    r10w,
    r11w,
    r12w,
    r13w,
    r14w,
    r15w,
    r8d,
    r9d,
    r10d,
    r11d,
    r12d,
    r13d,
    r14d,
    r15d,
    r8,
    r9,
    r10,
    r11,
    r12,
    r13,
    r14,
    r15,
}