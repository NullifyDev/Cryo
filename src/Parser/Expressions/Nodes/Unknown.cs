using System.Text.Json;

namespace Cryo;

public partial class Expression 
{
    public record Unknown(Token token, string file, int line, int col) : Node(file, line, col);
}