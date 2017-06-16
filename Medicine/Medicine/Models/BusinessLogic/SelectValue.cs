using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medicine.Models.BusinessLogic
{
    public class SelectValue
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        #region GetDoctors
        public SelectList GetDoctor(int? isSelected = null)
        {
            List<KeyValue> doctors = (from d in db.Doctors
                                      where d.IsVisible
                                      orderby d.Name
                                      select new KeyValue
                                      {
                                          Id = d.DoctorId,
                                          Name = d.Name + " " + d.Sername
                                         }).ToList();
            return new SelectList(doctors,"Id","Name",isSelected);
        }
        #endregion

        #region GetDoctorType
        public SelectList GetDoctorType(int? isSelected = null)
        {
            List<KeyValue> type = (from t in db.DoctorTypes
                                  where t.IsVisible
                                  orderby t.Name
                                  select new KeyValue
                                  {
                                      Id = t.DoctorTypeId,
                                      Name = t.Name
                                  }).ToList();

            return new SelectList(type,"Id","Name",isSelected);
        }


        #endregion

        #region GetTimetable
        public SelectList GetTimetable(int? isSelected = null)
        {
            List<KeyValue> time = (from t in db.Timetable
                                   where t.IsVisible == true
                                   select new KeyValue
                                   {
                                       Id = t.TimetableId,
                                       Name = t.Name

                                   }).ToList(); ;
            return new SelectList(time, "Id", "Name", isSelected);
        }
        #endregion

    }
}