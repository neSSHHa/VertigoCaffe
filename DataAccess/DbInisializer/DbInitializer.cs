using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Utility;
using Models.Models;
using DataAccess.Context;
using Type = Models.Models.Type;

namespace VertigoCaffe.DataAccess.DbInitializer
{
	public class DbInitializer : IDbInitializer
	{

		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ApplicationDbContext _db;


		public DbInitializer(
			UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager,
			ApplicationDbContext db
			)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_db = db;

		}

		public void Initialize()
		{

			// add migration if they arent all applied
			try
			{
				if (_db.Database.GetPendingMigrations().Count() > 0)
				{
					_db.Database.Migrate();
				}
			}
			catch (Exception ex)
			{

			}

			// add types
			if (!_db.Types.Any())
			{
				_db.Types.Add(new Type()
				{
					Name = "Kategorija"
				});
				_db.Types.Add(new Type()
				{
					Name = "Proizvod"
				});
				_db.Types.Add(new Type()
				{
					Name = "Dodatak"
				});
				_db.SaveChanges();
			}


			// add roles 

			if (!_roleManager.RoleExistsAsync(StaticDetails.ROLE_ADMIN).GetAwaiter().GetResult()) // seed it
			{
				_roleManager.CreateAsync(new IdentityRole(StaticDetails.ROLE_ADMIN)).GetAwaiter().GetResult();
				_roleManager.CreateAsync(new IdentityRole(StaticDetails.ROLE_MODERATOR)).GetAwaiter().GetResult();


				// creates user


				_userManager.CreateAsync(new ApplicationUser
				{
					UserName = "",
					NormalizedUserName = "",
					Email = "",
					NormalizedEmail = "",
					PhoneNumber = "",
					EmailConfirmed = true,
					PhoneNumberConfirmed = true,
					Id = "1",
					Image = @"\images\default.jpg"
				}, "").GetAwaiter().GetResult();
				ApplicationUser user = _db.Users.FirstOrDefault(x => x.Email == "");
				_userManager.AddToRoleAsync(user, StaticDetails.ROLE_ADMIN).GetAwaiter().GetResult();

			}
			return;



		}
	}
}