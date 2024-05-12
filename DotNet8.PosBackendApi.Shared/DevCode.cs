namespace DotNet8.PosBackendApi.Shared;

public static class DevCode
{
    public static string ToHash(this string password, string sharedKey)
    {
        return Hash.Create(HashType.SHA256, password, sharedKey, false);
    }

    public static DateTime ToDateTime(this DateTime? dateTime)
    {
        return Convert.ToDateTime(dateTime);    
    }

    public static string GenerateCode(this string code,string prefix,int length = 5)
    {
        string generateCode = string.Empty;
        if (string.IsNullOrWhiteSpace(code))
        {
            string defaultCode = "1";
            generateCode = $"{prefix}{defaultCode.PadLeft(length, '0')}";
            goto result;
        }

        //ode = code.Substring(1);
        code = code.Replace(prefix, "");
        int convertToInt = Convert.ToInt32(code) + 1;
        generateCode = $"{prefix}{convertToInt.ToString().PadLeft(length, '0')}";
        result:
        return generateCode;
    }

    public static string GenerateProductCategoryCode(this string code, string prefix, int length = 5)
    {
        string generateCode = string.Empty;
        if (string.IsNullOrWhiteSpace(code))
        {
            string defaultCode = "1";
            generateCode = $"{prefix}{defaultCode.PadLeft(length, '0')}";
            goto result;
        }

        //code = code.Substring(1);
        code = code.Replace(prefix, "");
        int convertToInt = Convert.ToInt32(code) + 1;
        generateCode = $"{prefix}{convertToInt.ToString().PadLeft(length, '0')}";
        result:
        return generateCode;
    }

    public static string GenerateTownshipCode(this string code, string prefix, int length = 5)
    {
        string generateCode = string.Empty;
        if (string.IsNullOrWhiteSpace(code))
        {
            string defaultCode = "1";
            generateCode = $"{prefix}{defaultCode.PadLeft(length, '0')}";
            goto result;
        }

        //code = code.Substring(1);
        code = code.Replace(prefix, "");
        int convertToInt = Convert.ToInt32(code) + 1;
        generateCode = $"{prefix}{convertToInt.ToString().PadLeft(length, '0')}";
        result:
        return generateCode;
    }

    public static string GenerateStateCode(this string code, string prefix, int length = 5)
    {
        string generateCode = string.Empty;
        if (string.IsNullOrWhiteSpace(code))
        {
            string defaultCode = "1";
            generateCode = $"{prefix}{defaultCode.PadLeft(length, '0')}";
            goto result;
        }

        //code = code.Substring(1);
        code = code.Replace(prefix, "");
        int convertToInt = Convert.ToInt32(code) + 1;
        generateCode = $"{prefix}{convertToInt.ToString().PadLeft(length, '0')}";
        result:
        return generateCode;
    }

    public static IQueryable<TSource> Pagination<TSource>(this IQueryable<TSource> source, int pageNo, int pageSize)
    {
        return source
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize);
    }
}