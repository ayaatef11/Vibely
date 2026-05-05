using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SocialMedia.Core.Context;
using System.Data.Common;
using static Dapper.SqlMapper;

namespace SocialMedia.Infrastructure.Repository;
public class MainRepository<TEnity> : IMainRepository<TEnity> where TEnity : class
{
    private readonly AppdbContext _context;
    private readonly DbSet<TEnity> _dbSet;
    private readonly IConfiguration _configuration;
    private readonly DbConnection connection;


    public MainRepository(AppdbContext context, IConfiguration configuration)
    {
        this._context = context;
        this._dbSet = context.Set<TEnity>();
        this._configuration = configuration;
        connection = _context.Database.GetDbConnection();
    }

    public async ValueTask<int> CreateAsync(TEnity entity)
    {
        var tableName = typeof(TEnity).Name;
        var properties = typeof(TEnity).GetProperties()
            .Where(p =>
                p.PropertyType.IsPrimitive ||
                p.PropertyType == typeof(string) ||
                p.PropertyType == typeof(Guid) ||
                p.PropertyType == typeof(DateTime) ||
                p.PropertyType == typeof(Guid?) ||
                p.PropertyType == typeof(int?) ||
                p.PropertyType == typeof(long?)
            );
        var columns = string.Join(",", properties.Select(p => p.Name));
        var values = string.Join(",", properties.Select(p => "@" + p.Name));

        var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({values}) ";

        var result = await connection.ExecuteAsync(sql, entity);

        return result ;
    }

    public async ValueTask<int> DeleteAsync(Guid id)
    {
        var tableName = typeof(TEnity).Name;
        var sql = $"DELETE FROM {tableName} WHERE Id =@Id";
        var result = await connection.ExecuteAsync(sql, new { Id = id });
        return result ;
    }

    public async ValueTask<IEnumerable<TEnity>> GetAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async ValueTask<TEnity> GetAsync(Guid id)
    {
        var Sql = "SELECT * FROM " + typeof(TEnity).Name + " WHERE ID = @Id";
        return await connection.QuerySingleOrDefaultAsync<TEnity>(Sql, new { Id = id });

    }

    public async ValueTask<int> UpdateAsync(TEnity entity, Guid id)
    {
        var tableName = typeof(TEnity).Name;

        var properties = typeof(TEnity)
            .GetProperties()
            .Where(p => (p.Name != "Id") &&
            (p.PropertyType.IsPrimitive ||
                p.PropertyType == typeof(string) ||
                p.PropertyType == typeof(Guid) ||
                p.PropertyType == typeof(DateTime) ||
                p.PropertyType == typeof(Guid?) ||
                p.PropertyType == typeof(int?) ||
                p.PropertyType == typeof(long?)));

        var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

        var sql = $"UPDATE {tableName} SET {setClause} WHERE Id = @Id";

        var parameters = new DynamicParameters(entity);
        parameters.Add("Id", id);

        var result = await connection.ExecuteAsync(sql, parameters);

        return result;
    }

}
