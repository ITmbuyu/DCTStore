using DCTStore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting; // Add this using directive
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using DCTStore.Repository;
using DCTStore.Models;

public class Program
{
    private readonly IWebHostEnvironment _env; // Add the private field
    private readonly CartItemRepository _cartItemRepository;

    public Program(IWebHostEnvironment env, CartItemRepository cartItemRepository) // Inject IWebHostEnvironment here
    {
        _env = env;
        _cartItemRepository = cartItemRepository; // Initialize the field
    }

    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
		builder.Services.AddDbContext<ApplicationDbContext>(options =>
		   options.UseSqlServer(connectionString, sqlServerOptions =>
		   {
			   sqlServerOptions.EnableRetryOnFailure();
		   }));
		builder.Services.AddDatabaseDeveloperPageExceptionFilter();

		builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddRoles<IdentityRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>();
		builder.Services.AddControllersWithViews();
		builder.Services.AddScoped<CartItemRepository>();
        builder.Services.AddHttpClient(); // Add this line
		builder.Services.AddScoped<Cart>();
		builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		// Add session services
		builder.Services.AddDistributedMemoryCache();
		builder.Services.AddSession(options =>
		{
			options.IdleTimeout = TimeSpan.FromMinutes(30);
			options.Cookie.HttpOnly = true;
			options.Cookie.IsEssential = true;
		});



		var app = builder.Build();

       

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

		// Use session middleware
		app.UseSession();

		app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.MigrateAsync(); // Apply any pending database migrations

            var roles = new[] { "Admin" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.MigrateAsync(); // Apply any pending database migrations

            string email = "admin@durbanchristiantabernacle.org";
            string password = "hello123@B4";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser();
                user.UserName = email;
                user.Email = email;
                user.EmailConfirmed = true;

                await userManager.CreateAsync(user, password);

                await userManager.AddToRoleAsync(user, "Admin");
            }
        }

        app.Run();
    }
}
