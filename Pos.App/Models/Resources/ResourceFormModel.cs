using System.Globalization;
using System.Text.Json;

namespace Pos.App.Models.Resources;

public sealed class ResourceFormModel
{
    public int Id { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductCategoryCode { get; set; }
    public string? ProductCategoryName { get; set; }
    public string? ProductName { get; set; }
    public decimal Price { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? StateCode { get; set; }
    public string? StateName { get; set; }
    public string? TownshipCode { get; set; }
    public string? TownshipName { get; set; }
    public string? StaffCode { get; set; }
    public string? StaffName { get; set; }
    public string? ShopCode { get; set; }
    public string? ShopName { get; set; }
    public string? MobileNo { get; set; }
    public string? Address { get; set; }
    public DateTime DateOfBirth { get; set; } = new(2000, 1, 1);
    public string? Gender { get; set; }
    public string? Position { get; set; }
    public string? Password { get; set; }
    public int FromAmount { get; set; }
    public int ToAmount { get; set; }
    public string? TaxType { get; set; }
    public decimal? Percentage { get; set; }
    public decimal? FixedAmount { get; set; }

    public object ToPayload(string slug) => slug switch
    {
        "product" => new { ProductId = Id, ProductCode, ProductCategoryCode, ProductName, Price },
        "product-category" => new { ProductCategoryId = Id, ProductCategoryCode, ProductCategoryName },
        "customer" => new { CustomerId = Id, CustomerCode, CustomerName, MobileNo, DateOfBirth, Gender, StateCode, TownshipCode },
        "state" => new { StateId = Id, StateCode, StateName },
        "township" => new { TownshipId = Id, TownshipCode, TownshipName, StateCode },
        "staff" => new { StaffId = Id, StaffCode, StaffName, DateOfBirth, MobileNo, Address, Gender, Position, Password },
        "tax" => new { TaxId = Id, FromAmount, ToAmount, TaxType, Percentage, FixedAmount },
        "shop" => new { ShopId = Id, ShopCode, ShopName, MobileNo, Address },
        _ => throw new InvalidOperationException("Unknown resource."),
    };

    public string? GetValue(string property) => property switch
    {
        nameof(ProductCode) => ProductCode,
        nameof(ProductCategoryCode) => ProductCategoryCode,
        nameof(ProductCategoryName) => ProductCategoryName,
        nameof(ProductName) => ProductName,
        nameof(Price) => Price.ToString(CultureInfo.InvariantCulture),
        nameof(CustomerCode) => CustomerCode,
        nameof(CustomerName) => CustomerName,
        nameof(StateCode) => StateCode,
        nameof(StateName) => StateName,
        nameof(TownshipCode) => TownshipCode,
        nameof(TownshipName) => TownshipName,
        nameof(StaffCode) => StaffCode,
        nameof(StaffName) => StaffName,
        nameof(ShopCode) => ShopCode,
        nameof(ShopName) => ShopName,
        nameof(MobileNo) => MobileNo,
        nameof(Address) => Address,
        nameof(DateOfBirth) => DateOfBirth.ToString("yyyy-MM-dd"),
        nameof(Gender) => Gender,
        nameof(Position) => Position,
        nameof(Password) => Password,
        nameof(FromAmount) => FromAmount.ToString(CultureInfo.InvariantCulture),
        nameof(ToAmount) => ToAmount.ToString(CultureInfo.InvariantCulture),
        nameof(TaxType) => TaxType,
        nameof(Percentage) => Percentage?.ToString(CultureInfo.InvariantCulture),
        nameof(FixedAmount) => FixedAmount?.ToString(CultureInfo.InvariantCulture),
        _ => null,
    };

    public void Load(JsonElement item)
    {
        Id = GetInt(item, "productId", "productCategoryId", "customerId", "stateId", "townshipId", "staffId", "taxId", "shopId");
        ProductCode = GetString(item, "productCode");
        ProductCategoryCode = GetString(item, "productCategoryCode");
        ProductName = GetString(item, "productName");
        ProductCategoryName = GetString(item, "productCategoryName");
        Price = GetDecimal(item, "price");
        CustomerCode = GetString(item, "customerCode");
        CustomerName = GetString(item, "customerName");
        StateCode = GetString(item, "stateCode");
        StateName = GetString(item, "stateName");
        TownshipCode = GetString(item, "townshipCode");
        TownshipName = GetString(item, "townshipName");
        StaffCode = GetString(item, "staffCode");
        StaffName = GetString(item, "staffName");
        ShopCode = GetString(item, "shopCode");
        ShopName = GetString(item, "shopName");
        MobileNo = GetString(item, "mobileNo");
        Address = GetString(item, "address");
        Gender = GetString(item, "gender");
        Position = GetString(item, "position");
        TaxType = GetString(item, "taxType");
        FromAmount = GetInt(item, "fromAmount");
        ToAmount = GetInt(item, "toAmount");
        Percentage = GetNullableDecimal(item, "percentage");
        FixedAmount = GetNullableDecimal(item, "fixedAmount");
        if (item.TryGetProperty("dateOfBirth", out var dob) && dob.TryGetDateTime(out var date)) DateOfBirth = date;
    }

    private static string? GetString(JsonElement item, string name) =>
        item.TryGetProperty(name, out var value) && value.ValueKind != JsonValueKind.Null ? value.ToString() : null;
    private static int GetInt(JsonElement item, params string[] names)
    {
        foreach (var name in names)
            if (item.TryGetProperty(name, out var value) && value.TryGetInt32(out var number)) return number;
        return 0;
    }
    private static decimal GetDecimal(JsonElement item, string name) =>
        item.TryGetProperty(name, out var value) && value.TryGetDecimal(out var number) ? number : 0;
    private static decimal? GetNullableDecimal(JsonElement item, string name) =>
        item.TryGetProperty(name, out var value) && value.TryGetDecimal(out var number) ? number : null;
}
