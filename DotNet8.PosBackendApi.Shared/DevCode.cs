namespace DotNet8.PosBackendApi.Shared;

public static class DevCode
{
    public static string ToHash(this string password, string sharedKey)
    {
        return  Hash.Create(HashType.SHA256, password, sharedKey, false);
    }
}

public class DapperService
{
    private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder;

    public DapperService(string connectionString)
    {
        _sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
    }

    public List<T> Query<T>(string query, object? parameters = null)
    {
        using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
        List<T> lst = db.Query<T>(query).ToList();
        return lst;
    }

    public T QueryStoredProcedure<T>(string query, object? parameters = null)
    {
        using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
        var item = db.Query<T>(query, parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
        return item!;
    }

    public T QueryFirstOrDefault<T>(string query, object? parameters = null)
    {
        using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
        T item = db.QueryFirstOrDefault<T>(query)!;
        return item;
    }

    public int Execute(string query, object? parameters = null)
    {
        using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
        int result = db.Execute(query, parameters);
        return result;
    }
}
