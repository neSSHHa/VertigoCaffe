using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		Task<T> FirstOrDefaultAsync(Expression<Func<T,bool>> filter,bool tracked = true,string? included = null);
		IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? included = null);
		Task AddAsync(T entity);
		void Remove(T entity);
		void RemoveAll(IEnumerable<T> entites);

	}
}
