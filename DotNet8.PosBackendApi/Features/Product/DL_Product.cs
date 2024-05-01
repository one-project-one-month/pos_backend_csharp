namespace DotNet8.PosBackendApi.Features.Product;

public class DL_Product
{
    private readonly AppDbContext _context;

    public DL_Product(AppDbContext context) => _context = context;

    public async Task<ProductListResponseModel> GetProduct()
    {
        var responseModel = new ProductListResponseModel();
        try
        {
            var products = await _context
                .TblProducts
                .AsNoTracking()
                .ToListAsync();
            responseModel.DataLst = products
                .Select(x => x.Change())
                .ToList();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.DataLst = new List<ProductModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<ProductResponseModel> GetProductByCode(string productCode)
    {
        var responseModel = new ProductResponseModel();
        try
        {
            var product = await _context
                .TblProducts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductCode == productCode);
            if (product is null)
            {
                responseModel.MessageResponse = new MessageResponseModel
                    (false, EnumStatus.NotFound.ToString());
                goto result;
            }

            responseModel.Data = product.Change();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.Data = new ProductModel();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        result:
        return responseModel;
    }

    public async Task<MessageResponseModel> Create(ProductModel requestModel)
    {
        var responseModel = new MessageResponseModel();
        try
        {
            await _context.TblProducts.AddAsync(requestModel.Change());
            var result = await _context.SaveChangesAsync();
            responseModel = result > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<MessageResponseModel> Update(int id, ProductModel requestModel)
    {
        var responseModel = new MessageResponseModel();
        try
        {
            var item = await _context.TblProducts.FirstOrDefaultAsync(x => x.ProductId == id);

            if (item is null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                return responseModel;
            }

            if (!string.IsNullOrEmpty(requestModel.ProductCode))
            {
                item.ProductCode = requestModel.ProductCode;
            }

            if (!string.IsNullOrEmpty(requestModel.ProductName))
            {
                item.ProductName = requestModel.ProductName;
            }

            if (!string.IsNullOrEmpty(requestModel.ProductCategoryCode))
            {
                item.ProductCategoryCode = requestModel.ProductCategoryCode;
            }

            if (requestModel.Price > 0)
            {
                item.Price = requestModel.Price;
            }

            _context.TblProducts.Update(item);
            var result = await _context.SaveChangesAsync();

            responseModel = result > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<MessageResponseModel> Delete(int id)
    {
        var responseModel = new MessageResponseModel();
        try
        {
            var product = await _context
                .TblProducts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductId == id);
            if (product == null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }

            _context.TblProducts.Remove(product);
            var result = await _context.SaveChangesAsync();
            responseModel = result > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }

        result:
        return responseModel;
    }
}