using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
	public class TypeRepository : Repository<Models.Models.Type>, ITypeRepository
	{
		public TypeRepository(ApplicationDbContext db) : base(db)
		{

		}
	}
}
