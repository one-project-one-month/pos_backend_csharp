namespace DotNet8.PosFrontendBlazor.Services
{
    public static class Endpoints
    {
        public static string ProductCategory { get; } = "api/v1/product-categories";
        public static string Customer { get; } = "api/v1/customers";
        public static string Product { get; } = "api/v1/products";
        public static string Staff { get; } = "api/v1/staffs";
        public static string Township { get; } = "api/v1/townships";
        public static string State { get; } = "api/v1/state";
        public static string WithPagination(this string url, int pageNo, int pageSize)
        {
            return $"{url}/{pageNo}/{pageSize}";
        }
    }
}
