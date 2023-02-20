namespace Bot.Core.Extensions;

public static class StreamExtensions
{
    public static void WriteBinaryTo(this Stream source, Stream dest, int bufferSize = 1024)
    {
        Span<byte> buffer = stackalloc byte[bufferSize];
        int readed;
        while ((readed = source.Read(buffer)) > 0)
        {
            if (buffer.Length > readed)
                dest.Write(buffer[..readed]);
            else
                dest.Write(buffer);
        }
    }
}
