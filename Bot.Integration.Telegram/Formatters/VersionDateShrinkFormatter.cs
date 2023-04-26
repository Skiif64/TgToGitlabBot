using System.Text.RegularExpressions;


namespace Bot.Integration.Telegram.Formatters;
internal class VersionDateShrinkFormatter : IFormatter
{
    private const string Pattern = @"_v\d+(_\d+)*.+$";    
    public string Format(string message)
    {
        var extension = Path.GetExtension(message);
        var filename = Path.GetFileNameWithoutExtension(message);
        var result = Regex.Replace(filename, Pattern, "");
        return result + extension;
    }
}
