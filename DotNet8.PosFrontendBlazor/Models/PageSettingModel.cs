namespace DotNet8.PosFrontendBlazor.Models;

public class PageSettingModel
{
    public PageSettingModel(int pageNo, int pageSize, int pageCount, int totalCount)
    {
        PageNo = pageNo;
        PageSize = pageSize;
        PageCount = pageCount;
        TotalCount = totalCount;
    }
    public int TotalCount { get; set; }
    public int PageCount { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
    public bool IsEndOfPage => PageNo == PageCount;
}
