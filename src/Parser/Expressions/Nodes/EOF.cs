namespace Cryo;

public partial class Expression 
{
    public record EOF(string file, int line, int col) : Node(file, line, col);
}