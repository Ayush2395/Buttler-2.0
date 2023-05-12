using Buttler.Test.Domain.Enums;
using Buttler.Test.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Buttler.Test.Infrastructure.Persistance
{
    public class ApplicationDbInitialiser
    {
        private readonly ILogger<ApplicationDbInitialiser> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationDbInitialiser(ILogger<ApplicationDbInitialiser> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task InitializeDb()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while initializing database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while seeding into database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            try
            {
                var adminRole = new IdentityRole(Enums.UserRole.admin.ToString());
                var staffRole = new IdentityRole(Enums.UserRole.staff.ToString());


                if (_roleManager.Roles.All(r => r.Name != adminRole.Name))
                {
                    await _roleManager.CreateAsync(adminRole);
                }


                if (_roleManager.Roles.All(r => r.Name != staffRole.Name))
                {
                    await _roleManager.CreateAsync(staffRole);
                }

                var adminUser = new ApplicationUser { UserName = "Ayush", Email = "ayush@thepoweracademy.in", EmailConfirmed = true };

                if (_userManager.Users.All(r => r.UserName != adminUser.UserName))
                {
                    await _userManager.CreateAsync(adminUser, "Test@123");
                    if (!string.IsNullOrWhiteSpace(adminRole.Name))
                    {
                        await _userManager.AddToRolesAsync(adminUser, new[] { adminRole.Name });
                    }
                }

                //if (_context.UserDetails.Any(r => r.FirstName != adminUser.UserName))
                //{
                //    _context.UserDetails.Add(new()
                //    {
                //        UId = adminUser.Id,
                //        FirstName = adminUser.UserName,
                //        LastName = "Kumar",
                //        Age = 23,
                //        Gender = "Male"
                //    });
                //    await _context.SaveChangesAsync();
                //}

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while seeding user.");
                throw;
            }
        }
    }
}
