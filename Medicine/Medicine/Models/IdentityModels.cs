using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Medicine.Models.Doctors;
using Medicine.Models.Pacients;
using Medicine.Models.Schedules;

namespace Medicine.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DbConnection", throwIfV1Schema: false)
        {

        }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorType> DoctorTypes { get; set; }
        public DbSet<Pacient> Pacients { get; set; }
        public DbSet<DiseaseHistory> DiseaseHistories { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Registration> Registration { get; set; }
        public DbSet<Timetable> Timetable { get; set; }
        public DbSet<MeetTime> MeetTime { get; set; }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}