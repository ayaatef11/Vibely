namespace SocialMedia.Infrastructure.Repository;
public interface IMainRepository<TEntity> where TEntity : class
{
    ValueTask<TEntity> GetAsync(Guid Id);
    ValueTask<IEnumerable<TEntity>> GetAsync();
    ValueTask<int> DeleteAsync(Guid Id);
    ValueTask<int> CreateAsync(TEntity entity);
    ValueTask<int> UpdateAsync(TEntity entity, Guid Id);
}
