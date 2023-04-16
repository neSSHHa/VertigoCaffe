using DataAccess.Data;
using DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
	public class UnitOfWork : IUnitOFWork
	{
		private readonly ApplicationDbContext _db;
		public ICardRepsitory Card { get; private set; }
		public ITypeRepository Type { get; private set; }
		public IApplicationUserRepository ApplicationUser { get; private set; }

		public UnitOfWork(ApplicationDbContext db)
		{
			_db = db;
			Card = new CardRepository(_db);
			Type = new TypeRepository(_db);
			ApplicationUser = new ApplicationUserRepository(_db);
		}

		public async Task SaveAsync()
		{
			await _db.SaveChangesAsync();
		}
	}
}
