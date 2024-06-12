namespace DotNet8.PosFrontendBlazor.Server.Models.Custom;

public class KeyValueModel
{
    public string Key { get; set; }
    public string Value { get; set; }

    public KeyValueModel(string key, string value)
    {
        Key = key;
        Value = value;
    }
}