namespace DotNet8.PosBackendApi.Features.Generate;

[Route("api/[controller]")]
[ApiController]
public class GenerateController : BaseController
{
    private readonly ResponseModel _response;
    private readonly BL_SaleInvoice _saleInvoice;
    private readonly BL_ProductCategory _productCategory;
    private readonly BL_Product _product;

    public GenerateController(IServiceProvider serviceProvider, ResponseModel response, BL_SaleInvoice saleInvoice,
        BL_ProductCategory productCategory, BL_Product product) : base(serviceProvider)
    {
        _response = response;
        _saleInvoice = saleInvoice;
        _productCategory = productCategory;
        _product = product;
    }

    [Route("sale-invoice/{year}")]
    [HttpPost]
    public async Task<IActionResult> Execute(int year)
    {
        SaleInvoiceResponseModel responseModel = new SaleInvoiceResponseModel();
        SaleInvoiceModel requestModel = new SaleInvoiceModel();
        Random rnd = new Random();
        DateTime startDate = new DateTime(year, 01, 01);
        DateTime endDate = new DateTime(year, 12, 31);
        //DateTime endDate = new DateTime(year, 01, 03);
        for (DateTime date = startDate.Date; endDate.CompareTo(date) >= 0; date = date.AddDays(1))
        {
            var infoRecordCount = rnd.Next(1, 10);
            for (int i = 0; i < infoRecordCount; i++)
            {
                requestModel = new SaleInvoiceModel
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
                    SaleInvoiceDetailModel detail = new SaleInvoiceDetailModel();
                    detail.ProductCode = "P" + (j + 1).ToString("00000");
                    detail.Quantity = quantity;
                    detail.Price = _product.GetProductByCode(detail.ProductCode).Result.Data.Price;
                    detail.Amount = detail.Quantity * detail.Price;
                    requestModel.SaleInvoiceDetails.Add(detail);
                }

                requestModel.TotalAmount = requestModel.SaleInvoiceDetails.Sum(x => x.Amount);
                responseModel = await _saleInvoice.CreateSaleInvoice(requestModel);
            }
        }

        var model = _response.Return(new ReturnModel
        {
            Token = RefreshToken(),
            EnumPos = EnumPos.SaleInvoice,
            IsSuccess = responseModel.MessageResponse.IsSuccess,
            Message = responseModel.MessageResponse.Message
        });
        return Content(model);
    }

    [Route("product-categories")]
    [HttpPost]
    public async Task<IActionResult> ImportProductCategory()
    {
        int count = 1;
        var ProCategories = new[]
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
        foreach (var item in ProCategories)
        {
            await _productCategory.CreateProductCategory(new ProductCategoryModel
            {
                ProductCategoryCode = "PC_" + count.ToString("00000"),
                ProductCategoryName = item
            });
            count++;
        }

        return Content(new ReturnModel
        {
            Token = RefreshToken(),
            EnumPos = EnumPos.ProductCategory,
            IsSuccess = true,
            Message = "Success"
        });
    }

    [Route("products")]
    [HttpPost]
    public async Task<IActionResult> ImportProduct()
    {
        Random rnd = new Random();
        int count = 1;
        var Products = new[]
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
            "Salak (Snake fruit)",
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

        return Content(new ReturnModel
        {
            Token = RefreshToken(),
            EnumPos = EnumPos.ProductCategory,
            IsSuccess = true,
            Message = "Success"
        });
    }
}