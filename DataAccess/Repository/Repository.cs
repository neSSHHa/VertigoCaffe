using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
	public class Repository<T> : IRepository<T> where T: class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> _dbset;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _dbset = _db.Set<T>();
        }

		public async Task AddAsync(T entity)
		{
			await _db.AddAsync(entity);
		}

		public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, bool tracked = true, string? included = null)
		{
			IQueryable<T> query;
			if(tracked)
			{
				query = _dbset;
			}
			else
			{
				query = _dbset.AsNoTracking();
			}
			query = query.Where(filter);
			if (included != null)
			{
				foreach (var includedItem in included.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includedItem);
				}
			}
			return await query.FirstOrDefaultAsync();

		}

		public IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? included = null)
		{
			IQueryable<T> query;
			if (tracked)
			{
				query = _dbset;
			}
			else
			{
				query = _dbset.AsNoTracking();
			}

			if(filter != null)
			query = query.Where(filter);

			if (included != null)
			{
				foreach (var includedItem in included.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includedItem);
				}
			}
			return query;
		}

		public void Remove(T entity)
		{
			_db.Remove(entity);
		}

		public void RemoveAll(IEnumerable<T> entites)
		{
			_db.RemoveRange(entites);
		}
	}

}
