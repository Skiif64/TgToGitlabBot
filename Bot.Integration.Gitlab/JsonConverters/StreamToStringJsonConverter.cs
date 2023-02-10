using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bot.Integration.Gitlab.JsonConverters;

internal class StreamToStringJsonConverter : JsonConverter<Stream>
{
    public override Stream? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, Stream value, JsonSerializerOptions options)
    {
        using var sr = new StreamReader(value);
        writer.WriteStringValue(sr.ReadToEnd());
    }
}
