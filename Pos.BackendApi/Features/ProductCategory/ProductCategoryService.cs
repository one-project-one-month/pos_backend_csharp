namespace Pos.BackendApi.Features.ProductCategory;

public class ProductCategoryService
{
    private readonly AppDbContext _context;

    public ProductCategoryService(AppDbContext context) => _context = context;

    public async Task<ProductCategoryListResponseModel> GetProductCategory()
    {
        var responseModel = new ProductCategoryListResponseModel();
        try
        {
            var lst = await _context
                .TblProductCategories
                .AsNoTracking()
                .ToListAsync();
            responseModel.DataList = lst
                .Select(x => x.Change())
                .ToList();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.DataList = new List<ProductCategoryModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<ProductCategoryListResponseModel> GetProductCategory(int pageNo, int pageSize, string? search = null)
    {
        var responseModel = new ProductCategoryListResponseModel();
        try
        {
            var query = _context
                .TblProductCategories
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.ProductCategoryCode.Contains(search) ||
                    x.ProductCategoryName.Contains(search));
            }

            var totalCount = await query.CountAsync();
            var pageCount = totalCount / pageSize;
            if (totalCount % pageSize > 0)
                pageCount++;

            var lst = await query
                .Pagination(pageNo, pageSize)
                .ToListAsync();

            responseModel.Data = new ProductCategoryDataModel
            {
                ProductCategory = lst.Select(x => x.Change()).ToList(),
                PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount)
            };
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.DataList = new List<ProductCategoryModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<ProductCategoryResponseModel> GetProductCategoryByCode(string productCategoryCode)
    {
        if (productCategoryCode is null) throw new Exception("productCategoryCode is null");

        var responseModel = new ProductCategoryResponseModel();
        try
        {
            var item = await _context
                .TblProductCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductCategoryCode == productCategoryCode);
            if (item is null)
            {
                responseModel.MessageResponse = new MessageResponseModel
                    (false, EnumStatus.NotFound.ToString());
                goto result;
            }

            responseModel.Data = item.Change();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.Data = new ProductCategoryModel();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

    result:
        return responseModel;
    }

    public async Task<MessageResponseModel> CreateProductCategory(ProductCategoryModel requestModel)
    {
        CheckProductNullValue(requestModel);

        var responseModel = new MessageResponseModel();
        try
        {
            var productCategoryCode = await _context.TblProductCategories
                .AsNoTracking()
                .MaxAsync(x => x.ProductCategoryCode);
            requestModel.ProductCategoryCode = productCategoryCode.GenerateProductCategoryCode(EnumCodePrefix.PC_.ToString());
            await _context.TblProductCategories.AddAsync(requestModel.Change());
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

    public async Task<MessageResponseModel> UpdateProductCategory(int id, ProductCategoryModel requestModel)
    {
        if (id <= 0) throw new Exception("productCategoryCode is null");

        var responseModel = new MessageResponseModel();
        try
        {
            var item = await _context.TblProductCategories.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductCategoryId == id);

            if (item is null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                return responseModel;
            }

            if (!string.IsNullOrEmpty(requestModel.ProductCategoryCode))
                item.ProductCategoryCode = requestModel.ProductCategoryCode;

            if (!string.IsNullOrEmpty(requestModel.ProductCategoryName))
                item.ProductCategoryName = requestModel.ProductCategoryName;

            _context.TblProductCategories.Update(item);
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

    public async Task<MessageResponseModel> DeleteProductCategory(int id)
    {
        if (id <= 0) throw new Exception("productCategoryId is null");

        var responseModel = new MessageResponseModel();
        try
        {
            var item = await _context
                .TblProductCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductCategoryId == id);
            if (item is null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }

            _context.TblProductCategories.Remove(item);
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

    private void CheckProductNullValue(ProductCategoryModel productCategory)
    {
        if (productCategory == null)
            throw new Exception("productCategory is null.");

        if (string.IsNullOrWhiteSpace(productCategory.ProductCategoryName))
            throw new Exception("ProductCategoryName is null.");

        if (string.IsNullOrEmpty(productCategory.ProductCategoryCode))
            throw new Exception("ProductCateoryCode is null.");
    }
}
