using Medicine.Models.Pacients;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Medicine.Models.BusinessLogic
{
    public class PacientBL
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        #region Pacient
        public List<Pacient> ListPacient()
        {
            List<Pacient> pacient = db.Pacients.Where(x => x.IsVisible).ToList();
            return pacient;
        }

        public Pacient GetPacient(string name)
        {
            Pacient pacient = db.Pacients.Where(x => x.IsVisible && x.UserName == name).FirstOrDefault();
            return pacient;
        }

        public Pacient AddPacient(Pacient pacient, string userName)
        {
            if (pacient.Sex =="1")
            {
                pacient.Sex = "Male";
            }
            else
            {
                pacient.Sex = "Female";
            }           
            pacient.Email = userName;
            pacient.UserName = userName;
            pacient.IsVisible = true;
            pacient.WhoAdded = userName;
            pacient.WhenAdded = DateTime.Now;
            db.Pacients.Add(pacient);
            db.SaveChanges();
            return pacient;
        }
        public Pacient ChangePacient(Pacient pacient, string userName)
        {
            if (pacient.Sex == "1")
            {
                pacient.Sex = "Male";
            }
            else
            {
                pacient.Sex = "Female";
            }
            pacient.WhoModified = userName;
            pacient.WhenModified = DateTime.Now;
            db.Entry(pacient).State = EntityState.Modified;
            db.SaveChanges();
            return pacient;
        }
        public void DeletePacient(int id, string userName)
        {
            Pacient pacient = db.Pacients.Where(x => x.IsVisible && x.PacientId == id).FirstOrDefault();
            pacient.IsVisible = false;
            pacient.WhenDeleted = DateTime.Now;
            pacient.WhoDeleted = userName;
            db.Entry(pacient).State = EntityState.Modified;
            db.SaveChanges();
            ApplicationUser user = db.Users.Where(x => x.UserName == pacient.UserName).FirstOrDefault();
            user.EmailConfirmed = false;
            db.Entry(pacient).State = EntityState.Modified;
            db.SaveChanges();
        }
        #endregion
  
        #region PacientView
        public Pacient GetPacientByName(string name)
        {
            Pacient pacient = db.Pacients.Where(x => x.IsVisible && x.UserName == name).FirstOrDefault();
            return pacient;
        }
        #endregion
    }
}