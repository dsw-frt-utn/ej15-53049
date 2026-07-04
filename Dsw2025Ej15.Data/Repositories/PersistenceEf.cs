using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dsw2025Ej15.Domain;
using Dsw2025Ej15.Domain.Entities;
using Dsw2025Ej15.Data;

namespace Dsw2025Ej15.Data.Repositories
{
    public class PersistenceEf : IPersistence
    {
        private readonly ApplicationDbContext _context;

        public PersistenceEf(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T?> GetById<T>(Guid id) where T : EntityBase
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>?> GetAll<T>() where T : EntityBase
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> First<T>(Expression<Func<T, bool>> predicate) where T : EntityBase
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<T> Add<T>(T entity) where T : EntityBase
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Update<T>(T entity) where T : EntityBase
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Delete<T>(Guid id) where T : EntityBase
        {
            var entity = await GetById<T>(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
            return entity!;
        }
    }
}