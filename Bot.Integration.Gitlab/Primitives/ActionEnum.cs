using System.Runtime.Serialization;

namespace Bot.Integration.Gitlab.Primitives;

internal enum ActionEnum
{
    [EnumMember(Value = "create")]
    Create,
    [EnumMember(Value = "update")]
    Update,
    [EnumMember(Value = "delete")]
    Delete,
    [EnumMember(Value = "move")]
    Move,
    [EnumMember(Value = "chmod")]
    Chmod
}
