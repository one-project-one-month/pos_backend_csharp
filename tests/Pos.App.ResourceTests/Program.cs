using System.Text.Json;
using Pos.App.Models.Resources;

using var document = JsonDocument.Parse("""
{
  "data": {
    "product": [
      {
        "ProductCode": "P001",
        "ProductName": "Coffee",
        "Price": 2500
      }
    ]
  }
}
""");

var rows = JsonApiReader.GetRows(document);
AssertEqual(1, rows.Count, "Rows should be read from the API envelope.");
AssertEqual("P001", JsonApiReader.Display(rows[0], "productCode"), "Display should find existing properties regardless of JSON casing.");
AssertEqual("2,500.00", JsonApiReader.Display(rows[0], "price"), "Display should format existing numeric properties regardless of JSON casing.");
AssertEqual("—", JsonApiReader.Display(rows[0], "missing"), "Display should still fall back for missing properties.");

static void AssertEqual<T>(T expected, T actual, string message)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
        throw new InvalidOperationException($"{message} Expected '{expected}', got '{actual}'.");
}
