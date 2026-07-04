using Dsw2025Ej15.Domain;
using Dsw2025Ej15.Domain.Entities;
using System.Linq.Expressions;

namespace Dsw2025Ej15.Data.Repositories;

public class InMemory : IPersistence
{
    // Usamos una lista genérica vacía solo para que el proyecto compile
    public async Task<T?> GetById<T>(Guid id) where T : EntityBase
    {
        return await Task.FromResult(default(T));
    }

    public async Task<List<T>?> GetAll<T>() where T : EntityBase
    {
        return await Task.FromResult(new List<T>());
    }

    public async Task<T?> First<T>(Expression<Func<T, bool>> predicate) where T : EntityBase
    {
        return await Task.FromResult(default(T));
    }

    public async Task<T> Add<T>(T entity) where T : EntityBase
    {
        return await Task.FromResult(entity);
    }

    public async Task<T> Update<T>(T entity) where T : EntityBase
    {
        return await Task.FromResult(entity);
    }

    public async Task<T> Delete<T>(Guid id) where T : EntityBase
    {
        return await Task.FromResult(default(T)!);
    }
}