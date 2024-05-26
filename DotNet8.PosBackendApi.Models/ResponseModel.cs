using DotNet8.PosBackendApi.Models.Setup.PageSetting;

namespace DotNet8.PosBackendApi.Models;

public class ResponseModel
{
    //public object ReturnCommand(ReturnCommandModel model)
    //{
    //    JObject jsonObject = new JObject(
    //        new JProperty("message", model.message),
    //        new JProperty("isSuccess", model.isSuccess),
    //        new JProperty("data", model.item is null ? model.item :
    //            new JObject(
    //                new JProperty(model.enumPos.ToString().ToLower(), JToken.FromObject(model.item))
    //            )
    //        )
    //    );
    //    return jsonObject;
    //}

    //public object ReturnById(ReturnByIdModel model)
    //{
    //    JObject jsonObject = new JObject(
    //        new JProperty("message", model.message),
    //        new JProperty("isSuccess", model.isSuccess),
    //        new JProperty("data", new JObject(
    //                new JProperty(model.enumPos.ToString().ToLower(), JToken.FromObject(model.item))
    //            )
    //        )
    //    );
    //    return jsonObject;
    //}

    public object Return(ReturnModel model)
    {
        JObject jsonObject = new JObject(
            new JProperty("message", model.Message),
            new JProperty("token", model.Token),
            new JProperty("isSuccess", model.IsSuccess),
            new JProperty("data", model.Item is null ? model.Item : new JObject(
                    new JProperty(model.EnumPos.ToString().ToLower(), JToken.FromObject(model.Item))
                )
            )
            //new JProperty("bestsellerproductdata", model.BestSellerProduct is null ? model.BestSellerProduct : new JObject(
            //        new JProperty("bestsellerproduct", JToken.FromObject(model.BestSellerProduct))
            //    )
            //),
            //new JProperty("dailydashboarddata", model.DailyData is null ? model.DailyData : new JObject(
            //        new JProperty("dailydata", JToken.FromObject(model.DailyData))
            //    )
            //),
            //new JProperty("weeklydashboarddata", model.WeeklyData is null ? model.WeeklyData : new JObject(
            //        new JProperty("weeklydata", JToken.FromObject(model.WeeklyData))
            //    )
            //),
            //new JProperty("monthlydashboarddata", model.MonthlyData is null ? model.MonthlyData : new JObject(
            //        new JProperty("monthlydata", JToken.FromObject(model.MonthlyData))
            //    )
            //),
            //new JProperty("yearlydashboarddata", model.YearlyData is null ? model.YearlyData : new JObject(
            //        new JProperty("yearlydata", JToken.FromObject(model.YearlyData))
            //    )
            //)
        );
        if (model.Count is not null)
        {
            jsonObject.Add(new JProperty("result", model.Count));
        }
        if (model.PageSetting is not null)
        {
            jsonObject.Add(new JProperty("pageSetting", JToken.FromObject(model.PageSetting)));
        }
        return jsonObject;
    }
}

public class ReturnModel
{
    public string Token { get; set; }
    public string Message { get; set; }
    public int? Count { get; set; }
    public EnumPos EnumPos { get; set; }
    public bool IsSuccess { get; set; }
    public object? Item { get; set; }
    public PageSettingModel PageSetting { get; set; }
}