using System.Text;

namespace Ponto.Core;

public sealed class TeeTextWriter : TextWriter
{
    private readonly TextWriter _first;
    private readonly TextWriter _second;

    public TeeTextWriter(TextWriter first, TextWriter second)
    {
        _first = first;
        _second = second;
    }

    public override Encoding Encoding => _first.Encoding;

    public override void Write(char value)
    {
        _first.Write(value);
        _second.Write(value);
    }

    public override void Write(string? value)
    {
        _first.Write(value);
        _second.Write(value);
    }

    public override void WriteLine(string? value)
    {
        _first.WriteLine(value);
        _second.WriteLine(value);
    }

    public override void Flush()
    {
        _first.Flush();
        _second.Flush();
    }
}
