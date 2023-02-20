namespace Bot.Core.Extensions;

public static class StreamExtensions
{
    public static void WriteBinaryTo(this Stream source, Stream dest, int bufferSize = 1024)
    {
        Span<byte> buffer = stackalloc byte[bufferSize];
        int readed;
        while ((readed = source.Read(buffer)) > 0)
        {
            dest.Write(buffer[..readed]);
        }
    }

    public static async ValueTask WriteBinaryToAsync(this Stream source, Stream dest,
        int bufferSize = 1024, CancellationToken cancellationToken = default)
    {
        Memory<byte> buffer = new byte[bufferSize];
        int readed;
        while ((readed = await source.ReadAsync(buffer, cancellationToken)) > 0)
        {
            await dest.WriteAsync(buffer[..readed], cancellationToken);
        }
    }
}
