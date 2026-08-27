using System.Text.Json;

namespace Pos.App.Models.Resources;

public static class JsonApiReader
{
    public static JsonElement? GetDataItem(JsonDocument document)
    {
        if (!document.RootElement.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Object)
            return null;
        foreach (var property in data.EnumerateObject())
            return property.Value.Clone();
        return null;
    }

    public static IReadOnlyList<JsonElement> GetRows(JsonDocument document)
    {
        var item = GetDataItem(document);
        if (item is not { ValueKind: JsonValueKind.Array })
            return [];
        return item.Value.EnumerateArray().Select(value => value.Clone()).ToArray();
    }

    public static string Display(JsonElement row, string property)
    {
        if (!row.TryGetProperty(property, out var value) || value.ValueKind == JsonValueKind.Null)
            return "—";
        if (value.ValueKind == JsonValueKind.Number && value.TryGetDecimal(out var number))
            return number.ToString("N2");
        if (value.ValueKind == JsonValueKind.String && value.TryGetDateTime(out var date))
            return date.ToString("dd MMM yyyy");
        return value.ToString();
    }
}
