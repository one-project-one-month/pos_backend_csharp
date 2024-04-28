using DotNet8.PosBackendApi.Models.Setup.SaleInvoice;

namespace DotNet8.PosBackendApi.Features.Setup.SaleInvoice;

public class DL_SaleInvoice
{
    private readonly AppDbContext _context;
    private readonly DapperService _dapperService;

    public DL_SaleInvoice(AppDbContext context, DapperService dapperService)
    {
        _context = context;
        _dapperService = dapperService;
    }

    //public async Task<SaleInvoiceListResponseModel> GetSaleInvoice()
    //{
    //    var responseModel = new SaleInvoiceListResponseModel();
    //    try
    //    {
    //        var lst = await _context
    //            .TblSaleInvoices
    //            .AsNoTracking()
    //            .ToListAsync();
    //        if (lst == null)
    //        {
    //            responseModel.MessageResponse = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
    //            goto result;
    //        }
    //        foreach (var item in lst)
    //        {
    //            SaleInvoiceModel saleInvoiceModel = new SaleInvoiceModel();
    //            saleInvoiceModel = item.Change();
    //            var detailList = await _context
    //                .TblSaleInvoiceDetails
    //                .AsNoTracking()
    //                .Where(x => x.VoucherNo == item.VoucherNo)
    //                .ToListAsync();
    //            saleInvoiceModel.SaleInvoiceDetails = detailList.Select(x => x.Change()).ToList();
    //            responseModel.DataList.Add(saleInvoiceModel);
    //        }
    //        responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
    //    }
    //    catch (Exception ex)
    //    {
    //        responseModel.DataList = new List<SaleInvoiceModel>();
    //        responseModel.MessageResponse = new MessageResponseModel(false, ex);
    //    }
    //    result:
    //    return responseModel;
    //}

    public async Task<SaleInvoiceListResponseModel> GetSaleInvoice(DateTime startDate, DateTime endDate)
    {
        var responseModel = new SaleInvoiceListResponseModel();
        try
        {
            #region Get all sale invoice records

            var lstSaleInvoice = await _context
                .TblSaleInvoices
                .AsNoTracking()
                .Where(x => x.SaleInvoiceDateTime.Date >= startDate.Date && x.SaleInvoiceDateTime.Date <= endDate.Date)
                .ToListAsync();
            if (lstSaleInvoice == null)
            {
                responseModel.MessageResponse = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }

            #endregion

            #region Get all sale invoice detail records

            var lstVoucherNo = lstSaleInvoice.Select(x => x.VoucherNo).ToArray();
            var lstSaleInvoiceDetail = await _context.TblSaleInvoiceDetails.AsNoTracking().Where(x => lstVoucherNo.Contains(x.VoucherNo)).ToListAsync();
            if (lstSaleInvoiceDetail == null)
            {
                responseModel.MessageResponse = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }

            #endregion

            #region Prepare response model

            List<SaleInvoiceModel> saleInvoiceModels = lstSaleInvoice.Change(lstSaleInvoiceDetail);
            responseModel.DataList = saleInvoiceModels;
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());

            #endregion
        }
        catch (Exception ex)
        {
            responseModel.DataList = new List<SaleInvoiceModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }
        result:
        return responseModel;
    }

    public async Task<SaleInvoiceResponseModel> GetSaleInvoice(string voucherNo)
    {
        var responseModel = new SaleInvoiceResponseModel();
        try
        {
            var item = await _context
                .TblSaleInvoices
                .AsNoTracking()
                .Where(x => x.VoucherNo == voucherNo)
                .FirstOrDefaultAsync();
            if (item == null)
            {
                responseModel.MessageResponse = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }
            responseModel.Data = item.Change();
            var detailList = await _context
                .TblSaleInvoiceDetails
                .AsNoTracking()
                .Where(x => x.VoucherNo == item.VoucherNo)
                .ToListAsync();
            responseModel.Data.SaleInvoiceDetails = detailList.Select(x => x.Change()).ToList();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.Data = new SaleInvoiceModel();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }
        result:
        return responseModel;
    }

    // 1, 00001
    // 2, 00002
    // 3, 00003

    // 5
    // 00005
    // 00006

    // Id, Code, Letter, TotalNumber, Sequence
    // 1, SaleInvoice, VC_, 8, 4
    // VC_00000002
    public async Task<SaleInvoiceResponseModel> CreateSaleInvoice(SaleInvoiceModel model)
    {
        SaleInvoiceResponseModel responseModel = new SaleInvoiceResponseModel();
        try
        {
            // Generate Voucher Code
            //string voucherNo = $"VC_{DateTime.Now.ToString("yyyymmddhhmmssfff")}";
            string voucherNo = _dapperService.QueryStoredProcedure<string>("Sp_GenerateSaleInvoiceNo");
            model.VoucherNo = voucherNo;
            await _context.TblSaleInvoices.AddAsync(model.Change());
            await _context.TblSaleInvoiceDetails
                .AddRangeAsync(model.SaleInvoiceDetails!.Select(x => x.Change(voucherNo)).ToList());
            var result = await _context.SaveChangesAsync();
            responseModel.Data = result > 0? model : new SaleInvoiceModel();
            responseModel.MessageResponse = result > 0 ?
                new MessageResponseModel(true, EnumStatus.Success.ToString()) :
                new MessageResponseModel(false, EnumStatus.Fail.ToString());
            goto result;
        }
        catch (Exception ex)
        {
            responseModel.Data = new SaleInvoiceModel();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
            goto result;
        }
        result:
        return responseModel;
    }

    public async Task<MessageResponseModel> UpdateSaleInvoice(int id, SaleInvoiceModel model)
    {
        MessageResponseModel responseModel = new MessageResponseModel();
        try
        {
            var item = await _context
                .TblSaleInvoices
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SaleInvoiceId == id);
            if (item == null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }
            _context.TblSaleInvoices.Update(model.Change());
            var result = await _context.SaveChangesAsync();
            responseModel = result > 0 ?
                new MessageResponseModel(true, EnumStatus.Success.ToString()) :
                new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }
        result:
        return responseModel;
    }

    public async Task<MessageResponseModel> DeleteSaleInvoice(int id)
    {
        MessageResponseModel responseModel = new MessageResponseModel();
        try
        {
            var item = await _context
                .TblSaleInvoices
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SaleInvoiceId == id);
            if (item == null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }

            var lstDetail = await _context
                .TblSaleInvoiceDetails
                .AsNoTracking()
                .Where(x => x.VoucherNo == item.VoucherNo)
                .ToListAsync();
            if (lstDetail != null)
                _context.TblSaleInvoiceDetails.RemoveRange(lstDetail);

            _context.TblSaleInvoices.Remove(item);
            var result = await _context.SaveChangesAsync();
            responseModel = result > 0 ?
                new MessageResponseModel(true, EnumStatus.Success.ToString()) :
                new MessageResponseModel(false, EnumStatus.Fail.ToString());
            goto result;
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
            goto result;
        }
        result:
        return responseModel;
    }
}