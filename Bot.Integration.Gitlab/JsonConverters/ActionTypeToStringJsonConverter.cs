using Bot.Integration.Gitlab.Primitives.Base;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bot.Integration.Gitlab.JsonConverters;

internal class ActionTypeToStringJsonConverter : JsonConverter<ActionType>
{
    public override ActionType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, ActionType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}
