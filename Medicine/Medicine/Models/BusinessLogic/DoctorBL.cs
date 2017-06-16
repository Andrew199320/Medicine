using Medicine.Models;
using Medicine.Models.Doctors;
using Medicine.Models.Schedules;
using Microsoft.AspNet.Identity;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Medicine.Models.BusinessLogic
{
    public class DoctorBL
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        #region Doctor

        public List<Doctor> ListDoctors()
        {
            List<Doctor> doctors = db.Doctors.Where(x => x.IsVisible).OrderBy(x=>x.Name).ToList();
            return doctors;
        }
        
        public Doctor GetDoctor(int id)
        {
            Doctor doctor = db.Doctors.Where(x => x.IsVisible && x.DoctorId == id).FirstOrDefault();
            return doctor;
        }
        public Doctor AddDoctor(Doctor model, string userName)
        {
            if (model.Sex == "1")
            {
                model.Sex = "Male";
            }
            else
            {
                model.Sex = "Female";
            }
            model.UserName = model.Email;
            model.WhoAdded = userName;
            model.IsVisible = true;
            model.WhenAdded = DateTime.Now;
            db.Doctors.Add(model);
            db.SaveChanges();
            return model;
        }
        public void DeleteDoctor(int id, string userName)
        {
            Doctor doctor = db.Doctors.Where(x => x.IsVisible && x.DoctorId == id).FirstOrDefault();
            doctor.IsVisible = false;
            doctor.WhoDeleted = userName;
            doctor.WhenDeleted = DateTime.Now;
            db.Entry(doctor).State = EntityState.Modified;
            db.SaveChanges();

            ApplicationUser user = db.Users.Where(x => x.UserName == doctor.UserName).FirstOrDefault();
            user.EmailConfirmed = false;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }
        #endregion

        #region Doctor's page
        public Doctor GetDoctorByName(string name)
        {
            Doctor doctor = db.Doctors.Where(x => x.IsVisible && x.UserName == name).FirstOrDefault();
            return doctor;
        }
        public List<Schedule> GetScheduleById(int id)
        {
            var list = db.Schedule.Where(x => x.DoctorId == id).ToList();

            return list;
        }
        #endregion

        #region DoctorType
        public List<DoctorType> ListDoctorType()
        {
            List<DoctorType> list = db.DoctorTypes.Where(x => x.IsVisible).OrderBy(x=>x.Name).ToList();
            return list;
        }

        public DoctorType GetDoctorType(int id)
        {
            DoctorType type = db.DoctorTypes.Where(x => x.IsVisible && x.DoctorTypeId == id).FirstOrDefault();
            return type;
        }

        public DoctorType AddDoctorType(DoctorType model, string userName)
        {
            model.IsVisible = true;
            model.WhoAdded = userName;
            model.WhenAdded = DateTime.Now;
            db.DoctorTypes.Add(model);
            db.SaveChanges();
            return model;
        }
        public DoctorType EditDoctorType(DoctorType model, string userName)
        {
            model.WhoModified = userName;
            model.WhenModified = DateTime.Now;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return model;
        }
        public void DeleteDoctorType(int id, string userName)
        {
            DoctorType type = db.DoctorTypes.Where(x => x.IsVisible && x.DoctorTypeId == id).FirstOrDefault();
            type.IsVisible = false;
            type.WhoDeleted = userName;
            type.WhenDeleted = DateTime.Now;
            db.Entry(type).State = EntityState.Modified;
            db.SaveChanges();
        }
        #endregion
        
    }
}