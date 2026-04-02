namespace Cryo;

public partial class Expression 
{
    public record Header(string name, string file, int line, int col) : Node(file, line, col);
}