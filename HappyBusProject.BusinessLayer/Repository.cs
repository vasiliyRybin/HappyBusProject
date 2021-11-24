using HappyBusProject.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HappyBusProject.BusinessLayer
{
    public class Repository<T>:IRepository<T>
        where T:class
    {
        private readonly MyShuttleBusAppNewDBContext _context;

        public Repository(MyShuttleBusAppNewDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> Get()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public Task<T> Get(int id)
        {
            return null;//_context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Create(T dataObject)
        {
            _context.Set<T>().Add(dataObject);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T dataObject)
        {
            _context.Entry(dataObject).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
