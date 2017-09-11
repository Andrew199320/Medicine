namespace Art.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Art.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Art.Models.ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var roleAdmin = new IdentityRole { Name = "Admin" };
            var roleClient = new IdentityRole { Name = "Client" };
            var addAdmin = roleManager.Create(roleAdmin);
            var addClient = roleManager.Create(roleClient);
            context.SaveChanges();

            if (addAdmin.Succeeded)
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                ApplicationUser user;
                if (!context.Users.Any(x=>x.UserName == "admin@admin.com"))
                {
                    string password = "andrew";
                    string email = "admin@admin.com";
                    user = new ApplicationUser() { UserName = "admin", Email = email, EmailConfirmed = true };
                    if (userManager.Create(user,password).Succeeded)
                    {
                        userManager.AddToRole(user.Id, roleAdmin.Name);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
