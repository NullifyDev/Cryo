namespace Cryo;

public partial class Expression 
{
    public record SOF(string file, int line, int col) : Node(file, line, col);
}