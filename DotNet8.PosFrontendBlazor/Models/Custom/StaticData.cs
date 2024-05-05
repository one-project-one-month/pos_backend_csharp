namespace DotNet8.PosFrontendBlazor.Models.Custom
{
    public class StaticData
    {
        public static List<KeyValueModel> Gender()
        {
            return new List<KeyValueModel>()
            {
                new KeyValueModel("0", "--Select One--"),
                new KeyValueModel("Male", "Male"),
                new KeyValueModel("Female", "Female"),
            };
        }
    }
}
