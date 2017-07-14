namespace WebApplication1.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApplication1.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApplication1.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var roleNames = new[] { "Admin"};
            foreach(var roleName in roleNames) {
                if(!context.Roles.Any(r => r.Name == roleName)) {
                    // create role
                    var role = new IdentityRole { Name = roleName };
                    var result = roleManager.Create(role);
                    if(!result.Succeeded) {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var emails = new[] { "admin0@Gymbokning.se" };
            foreach(var email in emails) {
                if(!context.Users.Any(u => u.UserName == email)) {
                    var user = new ApplicationUser {
                        UserName = email,
                        Email = email,
                        FirstName= "a",
                        LastName="A",
                        EmailConfirmed= true,
                        PasswordHash = "0"
                        
                    };
                    var result = userManager.Create(user, "foobar");
                    if(!result.Succeeded) {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }
            }
            var adminUser = userManager.FindByName("admin0@Gymbokning.se");
            userManager.AddToRole(adminUser.Id, "Admin");

    }
    }
}
