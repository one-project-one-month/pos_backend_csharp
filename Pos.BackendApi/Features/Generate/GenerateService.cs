namespace Pos.BackendApi.Features.Generate;

public class GenerateService
{
    private readonly SaleInvoiceService _saleInvoice;
    private readonly ProductCategoryService _productCategory;
    private readonly ProductService _product;

    public GenerateService(
        SaleInvoiceService saleInvoice,
        ProductCategoryService productCategory,
        ProductService product)
    {
        _saleInvoice = saleInvoice;
        _productCategory = productCategory;
        _product = product;
    }

    public async Task<SaleInvoiceResponseModel> GenerateSaleInvoices(int year)
    {
        SaleInvoiceResponseModel responseModel = new SaleInvoiceResponseModel();
        Random rnd = new Random();
        DateTime startDate = new DateTime(year, 01, 01);
        DateTime endDate = new DateTime(year, 12, 31);
        for (DateTime date = startDate.Date; endDate.CompareTo(date) >= 0; date = date.AddDays(1))
        {
            var infoRecordCount = rnd.Next(1, 10);
            for (int i = 0; i < infoRecordCount; i++)
            {
                var requestModel = new SaleInvoiceModel
                {
                    SaleInvoiceDateTime = date,
                    TotalAmount = 0,
                    Discount = 0,
                    StaffCode = "S001",
                    Tax = 0
                };

                var detailRecordCount = rnd.Next(1, 3);
                for (int j = 0; j < detailRecordCount; j++)
                {
                    var quantity = rnd.Next(1, 10);
                    var detail = new SaleInvoiceDetailModel
                    {
                        ProductCode = "P" + (j + 1).ToString("00000"),
                        Quantity = quantity
                    };
                    var product = await _product.GetProductByCode(detail.ProductCode);
                    detail.Price = product.Data.Price;
                    detail.Amount = detail.Quantity * detail.Price;
                    requestModel.SaleInvoiceDetails!.Add(detail);
                }

                requestModel.TotalAmount = requestModel.SaleInvoiceDetails!.Sum(x => x.Amount);
                responseModel = await _saleInvoice.CreateSaleInvoice(requestModel);
            }
        }

        return responseModel;
    }

    public async Task<ReturnModel> ImportProductCategories()
    {
        int count = 1;
        foreach (var item in ProductCategories)
        {
            await _productCategory.CreateProductCategory(new ProductCategoryModel
            {
                ProductCategoryCode = "PC_" + count.ToString("00000"),
                ProductCategoryName = item
            });
            count++;
        }

        return new ReturnModel
        {
            EnumPos = EnumPos.ProductCategory,
            IsSuccess = true,
            Message = "Success"
        };
    }

    public async Task<ReturnModel> ImportProducts()
    {
        Random rnd = new Random();
        int count = 1;
        foreach (var item in Products)
        {
            var amount = rnd.Next(1, 99) * 100;
            await _product.Create(new ProductModel
            {
                ProductCategoryCode = "PC_00001",
                ProductCode = "P_" + count.ToString("00000"),
                ProductName = item,
                Price = amount
            });
            count++;
        }

        return new ReturnModel
        {
            EnumPos = EnumPos.ProductCategory,
            IsSuccess = true,
            Message = "Success"
        };
    }

    private static readonly string[] ProductCategories =
    {
        "Fruit", "Vegetable", "Dairy", "Meat", "Beverage",
        "Snack", "Bakery", "Frozen", "Canned", "Condiment",
        "Cereal", "Grains", "Pasta", "Seafood", "Sweets",
        "Sauce", "Spices", "Tea", "Coffee", "Juice",
        "Water", "Milk", "Cheese", "Eggs", "Poultry",
        "Bread", "Cake", "Cookies", "Ice Cream", "Yogurt",
        "Chips", "Popcorn", "Nuts", "Chocolate", "Candy",
        "Jam", "Mayonnaise", "Pickles", "Oil", "Vinegar",
        "Rice", "Noodles", "Soup", "Salad", "Pizza",
        "Wine", "Beer", "Soda", "Energy Drink", "Liquor",
        "Toothpaste", "Shampoo", "Soap", "Detergent", "Toilet Paper",
        "Towel", "Diapers", "Tissues", "Deodorant", "Lotion",
        "Shaving Cream", "Razor", "Shower Gel", "Sunscreen", "Perfume",
        "Dish Soap", "Hand Soap", "Trash Bags", "Paper Towels", "Candles",
        "Detergent", "Laundry Baskets", "Mop", "Broom", "Sponges",
        "Bucket", "Vacuum", "Iron", "Mop", "Broom",
        "Dustpan", "Waste Bin", "Blender", "Microwave", "Toaster",
        "Kettle", "Coffee Maker", "Food Processor", "Juicer", "Slow Cooker",
        "Rice Cooker", "Waffle Maker", "Grill", "Oven", "Stove",
        "Cutlery", "Dishes", "Glassware", "Cookware", "Bakeware",
        "Utensils", "Containers", "Tupperware", "Plates", "Bowls",
        "Cups", "Saucers", "Mugs", "Pans", "Pots",
        "Spoons", "Forks", "Knives", "Baking Sheets", "Mixing Bowls",
        "Chopping Board", "Can Opener", "Colander", "Strainer", "Grater",
        "Peeler", "Measuring Cups", "Measuring Spoons", "Whisk", "Spatula",
        "Tongs", "Ladle", "Skillet", "Casserole Dish", "Cake Pan",
        "Serving Tray", "Serving Utensils", "Cutting Board", "Salt", "Pepper"
    };

    private static readonly string[] Products =
    {
        "Apple",
        "Banana",
        "Orange",
        "Grapes",
        "Strawberry",
        "Mango",
        "Pineapple",
        "Watermelon",
        "Kiwi",
        "Peach",
        "Pear",
        "Cherry",
        "Blueberry",
        "Raspberry",
        "Blackberry",
        "Lemon",
        "Lime",
        "Papaya",
        "Cranberry",
        "Fig",
        "Pomegranate",
        "Avocado",
        "Guava",
        "Plum",
        "Coconut",
        "Passion fruit",
        "Dragon fruit",
        "Lychee",
        "Melon",
        "Apricot",
        "Persimmon",
        "Nectarine",
        "Tangerine",
        "Clementine",
        "Grapefruit",
        "Cantaloupe",
        "Honeydew",
        "Jackfruit",
        "Starfruit",
        "Kiwifruit",
        "Elderberry",
        "Mulberry",
        "Gooseberry",
        "Tamarind",
        "Plantain",
        "Lychee",
        "Ackee",
        "Quince",
        "Date",
        "Olive",
        "Acerola (Barbados cherry)",
        "Breadfruit",
        "Boysenberry",
        "Cactus pear (Prickly pear)",
        "Custard apple",
        "Durian",
        "Feijoa (Pineapple guava)",
        "Jabuticaba",
        "Longan",
        "Mangosteen",
        "Miracle fruit",
        "Noni",
        "Pawpaw",
        "Persimmon",
        "Rambutan",
        "Sapodilla",
        "Soursop",
        "Ugli fruit",
        "White currant",
        "Yangmei (Chinese bayberry)",
        "Horned melon (Kiwano)",
        "Jaboticaba",
        "Loquat",
        "Maracuja (Passionfruit)",
        "Miracle Berry",
        "Monstera Deliciosa (Swiss cheese plant fruit)",
        "Osage orange (Hedge apple)",
        "Pummelo",
        "Salak",
        "Sea buckthorn",
        "Surinam cherry",
        "Velvet apple",
        "Wampee",
        "Yuzu",
        "Cranberry",
        "Blackberry",
        "Elderberry",
        "Gooseberry",
        "Mulberry",
        "Raspberry",
        "Blueberry",
        "Boysenberry",
        "Currant",
        "Strawberry",
        "Guava",
        "Kiwi",
        "Kiwi",
        "Lychee",
        "Mango",
        "Papaya",
        "Pineapple",
        "Watermelon",
        "Orange",
        "Grapes",
        "Pear",
        "Cherry",
        "Lemon",
        "Lime",
        "Pomegranate",
        "Plum",
        "Avocado",
        "Dragon fruit",
        "Melon",
        "Fig",
        "Peach",
        "Apricot",
        "Banana",
        "Apple",
        "Passion fruit",
        "Coconut",
        "Tangerine",
        "Clementine",
        "Grapefruit",
        "Cantaloupe",
        "Honeydew",
        "Jackfruit",
        "Starfruit",
        "Kiwifruit",
        "Tamarind",
        "Plantain",
        "Ackee",
        "Quince",
        "Date",
        "Olive",
        "Breadfruit",
        "Cactus pear",
        "Durian",
        "Feijoa",
        "Jabuticaba",
        "Longan",
        "Mangosteen",
        "Miracle fruit",
        "Noni",
        "Pawpaw",
        "Rambutan",
        "Sapodilla",
        "Soursop",
        "Ugli fruit",
        "White currant",
        "Yangmei",
        "Horned melon",
        "Loquat",
        "Maracuja",
        "Miracle Berry",
        "Monstera Deliciosa",
        "Osage orange",
        "Pummelo",
        "Salak",
        "Sea buckthorn",
        "Surinam cherry",
        "Velvet apple",
        "Wampee",
        "Yuzu"
    };
}
