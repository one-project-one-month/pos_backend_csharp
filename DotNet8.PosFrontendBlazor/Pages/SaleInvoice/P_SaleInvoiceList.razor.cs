using DotNet8.PosFrontendBlazor.Models.SaleInvoice;
using DotNet8.PosFrontendBlazor.Models.State;
using DotNet8.PosFrontendBlazor.Pages.Township;
using Newtonsoft.Json;

namespace DotNet8.PosFrontendBlazor.Pages.SaleInvoice
{
    public partial class P_SaleInvoiceList
    {
        private SaleInvoiceListResponseModel? ResponseModel;
        private int pageNo = 1;
        private int pageSize = 10;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await InjectService.EnableLoading();
                await List();
                StateHasChanged();
                await InjectService.DisableLoading();
            }
        }

        private async Task List()
        {
            ResponseModel = await HttpClientService.ExecuteAsync<SaleInvoiceListResponseModel>
            (
                Endpoints.SaleInvoice.WithPagination(pageNo, pageSize),
                //Endpoints.SaleInvoice + "?startDate=2023-01-01&endDate=2023-04-01",
                EnumHttpMethod.Get
            );
            Console.WriteLine(JsonConvert.SerializeObject(ResponseModel));
        }

        private void Create()
        {
            Nav.NavigateTo("sale-invoice");
        }

        private async Task PageChanged(int i)
        {
            pageNo = i;
            await List();
        }

        //private async Task Popup(string title, string townshipCode = null)
        //{
        //    DialogResult result;
        //    if (townshipCode is null)
        //    {
        //        result = await InjectService.ShowModalBoxAsync<P_TownshipDialog>(title);
        //    }
        //    else
        //    {
        //        #region Get Township By Code

        //        var tspModel = await HttpClientService.ExecuteAsync<TownshipResponseModel>(Endpoints.Township + "/" + townshipCode, EnumHttpMethod.Get);
        //        if (tspModel != null && tspModel.Data != null)
        //        {
        //            var stateModel = await HttpClientService.ExecuteAsync<StateResponeModel>($"{Endpoints.State}/{tspModel.Data.Township.StateCode}", EnumHttpMethod.Get);
        //            if (stateModel is not null && stateModel.Data is not null && stateModel.Data.State is not null)
        //                tspModel.Data.Township.StateName = stateModel.Data.State.StateName;
        //        }

        //        #endregion

        //        #region Add Parameters

        //        var parameters = new DialogParameters<P_TownshipDialog>();
        //        parameters.Add(x => x.model, tspModel == null ? new TownshipModel() : tspModel.Data.Township);

        //        #endregion

        //        result = await InjectService.ShowModalBoxAsync<P_TownshipDialog>(title, parameters);
        //    }

        //    if (!result.Canceled)
        //    {
        //        await List();
        //        StateHasChanged();
        //    }
        //}

        //private async Task Delete(int townshipId, string townshipName)
        //{
        //    var parameters = new DialogParameters<P_TownshipDeleteDialog>();
        //    parameters.Add(x => x.contentText, $"Are you sure you want to delete {townshipName} township?");
        //    parameters.Add(x => x.buttonText, "Delete");
        //    parameters.Add(x => x.color, Color.Error);
        //    parameters.Add(x => x.townshipId, townshipId);

        //    var options = new MudBlazor.DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        //    var result = await InjectService.ShowModalBoxAsync<P_TownshipDeleteDialog>("Delete", parameters);
        //    if (!result.Canceled)
        //        await List();
        //}
    }
}
