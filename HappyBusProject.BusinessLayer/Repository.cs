using HappyBusProject.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly MyShuttleBusAppNewDBContext _context;

        public Repository(MyShuttleBusAppNewDBContext myShuttleBusAppNewDBContext)
        {
            _context = myShuttleBusAppNewDBContext ?? throw new ArgumentNullException(nameof(myShuttleBusAppNewDBContext));
        }

        public async Task<T> GetFirstOrDefault(Func<T, bool> predicate)
        {
            return await Task.Run(() => _context.Set<T>().FirstOrDefault(predicate));
        }

        public async Task<IEnumerable<T>> Get()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<bool> Create(T item)
        {
            _context.Set<T>().Add(item);
            var result = await _context.SaveChangesAsync();
            if (result > 0) return true;
            return false;
        }

        public async Task<bool> Delete(T item)
        {
            _context.Set<T>().Remove(item);
            _context.Entry(item).State = EntityState.Deleted;
            var result = await _context.SaveChangesAsync();
            if (result > 0) return true;
            return false;
        }

        public async Task<bool> Update(T ModifiedEntity)
        {
            _context.Set<T>().Update(ModifiedEntity);
            _context.Entry(ModifiedEntity).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            if (result > 0) return true;
            return false;
        }
    }
}
