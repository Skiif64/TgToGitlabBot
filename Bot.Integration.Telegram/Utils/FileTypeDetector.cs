namespace Bot.Integration.Telegram.Utils;

internal class FileTypeDetector : IDisposable
{
    private static readonly Dictionary<string, char> __controlChars = new()
    {
        {"NUL", (char)0},
        {"BS",  (char)8},
        {"CR",  (char)13},
        {"SUB", (char)26}
    };

    private static readonly char[] __utfChars = new[]
    {
        'о', 'О', 'е', 'Е', 'а', 'А', 'и', 'И', 'н', 'Н', 'т', 'Т', 'с', 'С', 'р', 'Р'
    };

    private readonly Stream _stream;
    private readonly StreamReader _reader;
    private bool _disposed;

    public FileTypeDetector(Stream stream)
    {
        _stream = stream;
        _reader = new StreamReader(stream);
    }

    public bool IsBinary()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(FileTypeDetector));
        var streamPosition = _stream.Position;

        int ch;
        while ((ch = _reader.Read()) != -1)
        {
            if (IsControlChar(ch))
            {
                _stream.Position = streamPosition;
                return true;
            }
        }
        _stream.Position = streamPosition;
        return false;
    }

    public bool IsUtf8Encoded()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(FileTypeDetector));
        var streamPosition = _stream.Position;

        int ch;
        while ((ch = _reader.Read()) != -1)
        {
            foreach (var utfChar in __utfChars)
            {
                if (ch == utfChar)
                {
                    _stream.Position = streamPosition;
                    return true;
                }
            }
        }

        _stream.Position = streamPosition;
        return false;
    }

    public void Dispose()
    {
        if (_disposed) return;
        _stream.Dispose();
        _reader.Dispose();
        _disposed = true;
    }


    private static bool IsControlChar(int ch)
    {
        return ch > __controlChars["NUL"] && ch < __controlChars["BS"]
            || ch > __controlChars["CR"] && ch < __controlChars["SUB"];
    }
}
