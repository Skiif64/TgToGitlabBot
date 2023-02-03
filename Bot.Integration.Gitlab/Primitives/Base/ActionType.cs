using Bot.Integration.Gitlab.JsonConverters;
using System.Text.Json.Serialization;

namespace Bot.Integration.Gitlab.Primitives.Base;

[JsonConverter(typeof(ActionTypeToStringJsonConverter))]
internal class ActionType
{
    public static ActionType Create => new ActionType("create");
    public static ActionType Update => new ActionType("update");
    public static ActionType Delete => new ActionType("delete");
    public static ActionType Move => new ActionType("move");
    public static ActionType Chmod => new ActionType("chmod");
    public string Value { get; }
    private ActionType(string value)
    {
        Value = value;
    }

}
