namespace Cryo;

public partial class Expression 
{
    public record Label(string name, string file, int line, int col) : Node(file, line, col);
}