using System.Reflection;
using System.Text;

namespace Cryo;

public partial class Transpiler 
{
    public class Handler()
    {
        public string code = "";
        public Dictionary<string, Type> instructions = typeof(Cryo.Instruction).GetNestedTypes(BindingFlags.Public)
        .ToDictionary(t => t.Name.ToLower());

        public Dictionary<string, string> modules = new()
        {
            { "core", ""}
        };

        public string? Handle(string cmd, Node[] args)
        {
            cmd = cmd.ToLower();
            // System.Console.WriteLine($"InstructionHandler: type: {cmd} {string.Join(", ", args)}");
            
            if (instructions.ContainsKey(cmd))
            {
                Type commandType = instructions[cmd];
                IInstruction commandInstance = (IInstruction)Activator.CreateInstance(commandType)!;
                return commandInstance.ToAsm(args);
            }
            System.Console.WriteLine($"unknown instruction {cmd}");
            return null;
        }

        public string? Handle(Expression.Instruction inst)
        {
            string type = inst.Type.ToString().ToLower();
            // System.Console.WriteLine($"InstructionHandler: type: {type}");

            if (instructions.ContainsKey(type))
            {
                Type commandType = instructions[type];
                IInstruction commandInstance = (IInstruction)Activator.CreateInstance(commandType)!;
                return commandInstance.ToAsm(new ReadOnlySpan<Node>(inst.Arguments));
            }
            System.Console.WriteLine($"unknown instruction {type}");
            return null;
        }

        public void GetMethod(string module)
        {
            


        }

        public static string GetValue(Node arg)
            => arg switch {
                Expression.Literal lit => (lit.Value ?? "0 ; null").ToString(),
                Expression.Register reg => reg.Type.ToString().ToLower(),
                Expression.Instruction inst => new Transpiler.Handler().Handle(inst),
                // Expression.Label here
                _ => ""
            } ?? "";
    }
}