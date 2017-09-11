namespace MyWeb.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyWeb.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MyWeb.Models.ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var roleAdmin = new IdentityRole { Name = "Admin" };
            var roleUser = new IdentityRole { Name = "User" };
            var admin = roleManager.Create(roleAdmin);
            var user = roleManager.Create(roleUser);
            context.SaveChanges();

            if (admin.Succeeded)
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                ApplicationUser appUser;
                if (!context.Users.Any(x=>x.UserName == "admin@admin.com"))
                {
                    string password = "andrew";
                    string email = "admin@admin.com";
                    appUser = new ApplicationUser { UserName = "Admin", Email = email, EmailConfirmed = true };
                    if (userManager.Create(appUser,password).Succeeded)
                    {
                        userManager.AddToRole(appUser.Id, roleAdmin.Name);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
