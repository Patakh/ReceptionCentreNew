namespace ReceptionCentreNew.Application.Persistence.Contracts;

internal interface IGenericRepository<T> where T : class
{
    public Task<T> Get(T entity);
    public Task<T> AddAsync(T entity);
    public Task<T> UpdateAsync(T entity);
    public Task<T> Delete(T entity);
}