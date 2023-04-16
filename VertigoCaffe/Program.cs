using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DataAccess.Repository.IRepository;
using DataAccess.Repository;
using DataAccess.DbInisializer;
using DataAccess.Data;

namespace VertigoCaffe
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddDbContext<ApplicationDbContext>(options => 
																options.UseSqlServer
																(builder.Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging());
			builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();
			builder.Services.AddScoped<IUnitOFWork, UnitOfWork>();
			builder.Services.AddScoped<IDbInitializer, DbInitializer>();
			builder.Services.AddRazorPages();
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
            builder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = $"/Identity/Account/Login";
				options.LogoutPath = $"/Identity/Account/Logout";
			});
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseHttpLogging();
			SeedDatabase();

			app.UseCors();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapRazorPages();
			app.MapControllerRoute(
				name: "default",
				pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

			app.Run();
			void SeedDatabase()
			{
				using (var scope = app.Services.CreateScope())
				{
					var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();
					dbInitializer.Initialize();
				}
			}
		}
	}
}