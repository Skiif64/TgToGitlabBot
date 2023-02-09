using System.IO;

namespace Bot.Integration.Telegram;

internal class FileTypeDetector
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
    
    public static bool IsBinary(Stream stream)
    {
        var streamPosition = stream.Position;
        using (var reader = new StreamReader(stream))
        {
            int ch;
            while ((ch = reader.Read()) != -1)
            {
                if (IsControlChar(ch))
                {
                    stream.Position = streamPosition;
                    return true;
                }
            }
        }
        stream.Position = streamPosition;
        return false;
    }

    public static bool IsUtf8Encoded(Stream stream)
    {
        var streamPosition = stream.Position;
        using (var reader = new StreamReader(stream))
        {
            int ch;
            while ((ch = reader.Read()) != -1)
            {
                foreach(var utfChar in __utfChars)
                {
                    if(ch == utfChar)
                    {
                        stream.Position = streamPosition;
                        return true;
                    }
                }
            }
        }
        stream.Position = streamPosition;
        return false;
    }
    

    private static bool IsControlChar(int ch)
    {
        return (ch > __controlChars["NUL"] && ch < __controlChars["BS"])
            || (ch > __controlChars["CR"] && ch < __controlChars["SUB"]);
    }
}
