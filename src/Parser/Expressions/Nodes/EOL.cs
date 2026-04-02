namespace Cryo;

public partial class Expression 
{
    public record EOL(string file, int line, int col) : Node(file, line, col);
}