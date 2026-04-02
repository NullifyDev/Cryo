using System.Diagnostics.Contracts;

public static class Transpiler
{
    public static List<CoreImports> coreImports = new();
    
}

public enum CoreImports
{
    Print,
    Input,
    Strlen,
    Exit
}