namespace Microsoft.AspNetCore.Components;

// Compatibility marker for the .NET 10 Blazor TempData API documented after the
// 10.0.11 reference pack. FlashMessageService supplies the same encrypted,
// one-request cookie semantics until the shared-framework attribute is available.
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class SupplyParameterFromTempDataAttribute : Attribute
{
    public string? Name { get; set; }
}
