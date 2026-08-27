namespace Pos.App.Models.Resources;

public sealed record ResourceField(string Property, string Label, string Type = "text", bool Required = true);

public sealed record ResourceDefinition(
    string Slug,
    string Title,
    string Endpoint,
    string IdProperty,
    string RouteKeyProperty,
    IReadOnlyList<ResourceField> Fields,
    IReadOnlyList<string> Columns);

public static class ResourceCatalog
{
    private static readonly Dictionary<string, ResourceDefinition> Definitions = new(StringComparer.OrdinalIgnoreCase)
    {
        ["product"] = new("product", "Products", "api/v1/products", "productId", "productCode",
            [new("ProductCode", "Product code"), new("ProductCategoryCode", "Category code"), new("ProductName", "Product name"), new("Price", "Price", "number")],
            ["productCode", "productName", "productCategoryCode", "price"]),
        ["product-category"] = new("product-category", "Product categories", "api/v1/product-categories", "productCategoryId", "productCategoryCode",
            [new("ProductCategoryCode", "Category code"), new("ProductCategoryName", "Category name")],
            ["productCategoryCode", "productCategoryName"]),
        ["customer"] = new("customer", "Customers", "api/v1/customers", "customerId", "customerCode",
            [new("CustomerCode", "Customer code"), new("CustomerName", "Customer name"), new("MobileNo", "Mobile number"), new("DateOfBirth", "Date of birth", "date"), new("Gender", "Gender"), new("StateCode", "State code"), new("TownshipCode", "Township code")],
            ["customerCode", "customerName", "mobileNo", "gender"]),
        ["state"] = new("state", "States", "api/v1/states", "stateId", "stateCode",
            [new("StateCode", "State code"), new("StateName", "State name")],
            ["stateCode", "stateName"]),
        ["township"] = new("township", "Townships", "api/v1/townships", "townshipId", "townshipCode",
            [new("TownshipCode", "Township code"), new("TownshipName", "Township name"), new("StateCode", "State code")],
            ["townshipCode", "townshipName", "stateCode"]),
        ["staff"] = new("staff", "Staff", "api/v1/staffs", "staffId", "staffId",
            [new("StaffCode", "Staff code"), new("StaffName", "Staff name"), new("DateOfBirth", "Date of birth", "date"), new("MobileNo", "Mobile number"), new("Address", "Address"), new("Gender", "Gender"), new("Position", "Position"), new("Password", "Password", "password")],
            ["staffCode", "staffName", "mobileNo", "position"]),
        ["tax"] = new("tax", "Tax rules", "api/v1/taxes", "taxId", "taxId",
            [new("FromAmount", "From amount", "number"), new("ToAmount", "To amount", "number"), new("TaxType", "Tax type"), new("Percentage", "Percentage", "number", false), new("FixedAmount", "Fixed amount", "number", false)],
            ["fromAmount", "toAmount", "taxType", "percentage", "fixedAmount"]),
        ["shop"] = new("shop", "Shops", "api/v1/shops", "shopId", "shopId",
            [new("ShopCode", "Shop code"), new("ShopName", "Shop name"), new("MobileNo", "Mobile number"), new("Address", "Address")],
            ["shopCode", "shopName", "mobileNo", "address"]),
    };

    public static ResourceDefinition Get(string slug) =>
        Definitions.TryGetValue(slug, out var value)
            ? value
            : throw new InvalidOperationException($"Unknown resource '{slug}'.");
}
