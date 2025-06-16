using Dsw2025Ej15.Domain.Entities;
using System.Linq.Expressions;

namespace Dsw2025Ej15.Domain;

public interface IRepository
{
    Task<T?> GetById<T>(Guid id) where T : EntityBase;
    Task<List<T>?> GetAll<T>() where T: EntityBase;
    Task<T?> First<T>(Expression<Func<T, bool>> predicate) where T : EntityBase;
    Task<T> Add<T>(T entity) where T : EntityBase;
    Task<T> Update<T>(T entity) where T : EntityBase;
    Task<T> Delete<T>(Guid id) where T : EntityBase;
}