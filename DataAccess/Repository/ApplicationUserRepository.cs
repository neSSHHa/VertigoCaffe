using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
	public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ApplicationUser applicationUser)
        {
            _db.Users.Update(applicationUser);
        }
		public void DetechUser(ApplicationUser applicationUser)
		{
			_db.Entry(applicationUser).State = EntityState.Detached;
		}
		public void DetachAllApplicationUsers()
		{
			// Retrieve all ApplicationUsers from the database
			var applicationUsers = _db.Set<ApplicationUser>().ToList();

			// Detach each ApplicationUser from the DbContext
			foreach (var applicationUser in applicationUsers)
			{
				_db.Entry(applicationUser).State = EntityState.Detached;
			}
		}
	}
}
