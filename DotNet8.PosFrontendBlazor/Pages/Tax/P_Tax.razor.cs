namespace DotNet8.PosFrontendBlazor.Pages.Tax
{
    public partial class P_Tax
    {
        public async Task CreatePopUp()
        {
            DialogResult dialogResult = await InjectService.ShowModalBoxAsync<P_CreateTaxDialog>("New Tax");
        }

        public async Task EditPopUp()
        {
            DialogResult dialogResult = await InjectService.ShowModalBoxAsync<P_EditTaxDialog>("Edit Tax");
        }

        public async Task DeletePopUp()
        {
            DialogResult result = await InjectService.ShowModalBoxAsync<P_DeleteTaxDialog>("Delete Tax");
        }
    }
}
