namespace Medicine.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Medicine.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Medicine.Models.ApplicationDbContext context)
        {

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
         
            var roleAdmin = new IdentityRole { Name = "Admin" };
            var roleDoctor = new IdentityRole { Name = "Doctor" };
            var roleUser = new IdentityRole { Name = "User" };
            var res = roleManager.Create(roleAdmin);
            var doctorRole = roleManager.Create(roleDoctor);
            var userRole = roleManager.Create(roleUser);
            context.SaveChanges();

            if (res.Succeeded)
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                ApplicationUser user;
                if (!context.Users.Any(x => x.UserName == "admin@admin.com"))
                {
                    string password = "andrew";
                    string email = "admin@admin.com";

                    user = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
                    if (userManager.Create(user, password).Succeeded)
                    {
                        userManager.AddToRole(user.Id, roleAdmin.Name);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
