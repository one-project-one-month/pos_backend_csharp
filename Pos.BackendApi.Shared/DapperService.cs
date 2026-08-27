namespace DotNet8.PosBackendApi.Shared;

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

    public async Task<(IEnumerable<T1>, IEnumerable<T2>)> QueryMultipleAsync<T1, T2>(string storedProcedure, object parameters = null)
    {
        using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
        using var multi = await db.QueryMultipleAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

        var result1 = (await multi.ReadAsync<T1>()).ToList();
        var result2 = (await multi.ReadAsync<T2>()).ToList();

        return (result1, result2);
    }

    public async Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>)> QueryMultipleAsync<T1, T2, T3, T4, T5>(string storedProcedure, object parameters = null)
    {
        using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
        using var multi = await db.QueryMultipleAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

        var result1 = (await multi.ReadAsync<T1>()).ToList();
        var result2 = (await multi.ReadAsync<T2>()).ToList();
        var result3 = (await multi.ReadAsync<T3>()).ToList();
        var result4 = (await multi.ReadAsync<T4>()).ToList();
        var result5 = (await multi.ReadAsync<T5>()).ToList();

        return (result1, result2, result3, result4, result5);
    }
}