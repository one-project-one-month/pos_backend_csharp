namespace Pos.BackendApi.Features.Product;

public class ProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context) => _context = context;

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

    public async Task<ProductListResponseModel> GetProduct(int pageNo, int pageSize, string? search = null)
    {
        var responseModel = new ProductListResponseModel();
        try
        {
            var query = _context
                .TblProducts
                .OrderByDescending(x => x.ProductId)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.ProductCode.Contains(search) ||
                    x.ProductName.Contains(search));
            }

            var products = await query
                .Pagination(pageNo, pageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();
            var pageCount = totalCount / pageSize;

            if (totalCount % pageSize > 0)
                pageCount++;

            responseModel.DataLst = products.Select(x => x.Change()).ToList();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
            responseModel.PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
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
        if (productCode is null) throw new Exception("productCode is null");

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
        CheckProductNullValue(requestModel);

        var responseModel = new MessageResponseModel();
        try
        {
            var productCode = await _context.TblProducts
                .AsNoTracking()
                .MaxAsync(x => x.ProductCode);
            requestModel.ProductCode = productCode.GenerateCode(EnumCodePrefix.P.ToString());
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
        if (id <= 0) throw new Exception("productCode is null");

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
                item.ProductCode = requestModel.ProductCode;

            if (!string.IsNullOrEmpty(requestModel.ProductName))
                item.ProductName = requestModel.ProductName;

            if (!string.IsNullOrEmpty(requestModel.ProductCategoryCode))
                item.ProductCategoryCode = requestModel.ProductCategoryCode;

            if (requestModel.Price > 0)
                item.Price = requestModel.Price;

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
        if (id <= 0) throw new Exception("productCode is null");

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

    private static void CheckProductNullValue(ProductModel product)
    {
        if (product == null)
            throw new Exception("product is null.");

        if (string.IsNullOrWhiteSpace(product.ProductName))
            throw new Exception("product.ProductName is null.");

        if (string.IsNullOrWhiteSpace(product.ProductCategoryCode))
            throw new Exception("product.ProductCategoryCode is null.");

        if (product.Price <= 0)
            throw new Exception("product.Price must be greater than zero.");
    }
}
