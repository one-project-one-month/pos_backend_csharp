namespace DotNet8.PosBackendApi.Features.Setup.ProductCategory;

public class DL_ProductCategory
{
    private readonly AppDbContext _context;

    public DL_ProductCategory(AppDbContext context)
    {
        _context = context;
    }

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

    public async Task<ProductCategoryResponseModel> GetProductCategoryByCode(string productCategoryCode)
    {
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
        var responseModel = new MessageResponseModel();
        try
        {
            await _context.TblProductCategories.AddAsync(requestModel.Change());
            var result = await _context.SaveChangesAsync();
            responseModel = result > 0 ?
                new MessageResponseModel(true, EnumStatus.Success.ToString())
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
            _context.TblProductCategories.Update(requestModel.Change());
            var result = await _context.SaveChangesAsync();
            responseModel = result > 0 ?
                new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }
        result:
        return responseModel;
    }

    public async Task<MessageResponseModel> DeleteProductCategory(int id)
    {
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
            responseModel = result > 0 ?
                new MessageResponseModel(true, EnumStatus.Success.ToString())
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