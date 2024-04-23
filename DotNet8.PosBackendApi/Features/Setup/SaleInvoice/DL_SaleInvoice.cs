﻿using DotNet8.PosBackendApi.Models.Setup.SaleInvoice;

namespace DotNet8.PosBackendApi.Features.Setup.SaleInvoice
{
    public class DL_SaleInvoice
    {
        private readonly AppDbContext _context;

        public DL_SaleInvoice(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SaleInvoiceListResponseModel> GetSaleInvoice()
        {
            var responseModel = new SaleInvoiceListResponseModel();
            try
            {
                var lst = await _context
                    .TblSaleInvoices
                    .AsNoTracking()
                    .ToListAsync();
                if (lst == null)
                {
                    responseModel.MessageResponse = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                    goto result;
                }
                foreach (var item in lst)
                {
                    SaleInvoiceModel saleInvoiceModel = new SaleInvoiceModel();
                    saleInvoiceModel = item.Change();
                    var detailList = await _context
                        .TblSaleInvoiceDetails
                        .AsNoTracking()
                        .Where(x => x.VoucherNo == item.VoucherNo)
                        .ToListAsync();
                    saleInvoiceModel.SaleInvoiceDetails = detailList.Select(x => x.Change()).ToList();
                    responseModel.DataList.Add(saleInvoiceModel);
                }
                responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
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

        public async Task<MessageResponseModel> CreateSaleInvoice(SaleInvoiceModel model)
        {
            MessageResponseModel responseModel = new MessageResponseModel();
            try
            {
                // Generate Voucher Code
                string voucherNo = $"VC_{DateTime.Now.ToString("yyyymmddhhmmss")}";
                model.VoucherNo = voucherNo;
                await _context.TblSaleInvoices.AddAsync(model.Change());
                await _context.TblSaleInvoiceDetails
                    .AddRangeAsync(model.SaleInvoiceDetails.Select(x => x.Change(voucherNo)).ToList());
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
}