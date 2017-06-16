using Medicine.Models.Schedules;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Medicine.Models.BusinessLogic
{
    public class ScheduleBL
    {
        public readonly ApplicationDbContext db = new ApplicationDbContext();

        #region Timetabel
        public List<Timetable> ListTimetable()
        {
            List<Timetable> time = db.Timetable.Where(x => x.IsVisible).ToList();
            return time;
        }
        public Timetable GetTimetable(int id)
        {
            Timetable time = db.Timetable.Where(x => x.IsVisible && x.TimetableId == id).FirstOrDefault();
            return time;
        }
            
        public Timetable AddTimetable(Timetable time, string userName)
        {
            time.IsVisible = true;
            time.WhoAdded = userName;
            time.WhenAdded = DateTime.Now;
            db.Timetable.Add(time);
            db.SaveChanges();
            return time;
        }
        public Timetable ChangeTimetable(Timetable time, string userName)
        {
            time.WhoModified = userName;
            time.WhenModified = DateTime.Now;
            db.Entry(time).State = EntityState.Modified;
            db.SaveChanges();
            return time;
        }
        public void DeleteTimetable(int id, string userName)
        {
            Timetable time = db.Timetable.Where(x => x.IsVisible && x.TimetableId == id).FirstOrDefault();
            time.IsVisible = false;
            time.WhoDeleted = userName;
            time.WhenDeleted = DateTime.Now;
            db.Entry(time).State = EntityState.Modified;
            db.SaveChanges();
        }
        #endregion

        #region MeetTime
        public List<MeetTime> ListMeetTime()
        {
            List<MeetTime> list = db.MeetTime.Where(x => x.IsVisible).ToList();
            return list;
        }
        public MeetTime GetMeetTime(int id)
        {
            MeetTime time = db.MeetTime.Where(x => x.IsVisible && x.MeetTimeId == id).FirstOrDefault();
            return time;
        }
        public MeetTime AddMeetTime(MeetTime time, string userName, int id)
        {
            time.IsVisible = true;
            time.WhoAdded = userName;
            time.WhenAdded = DateTime.Now;
            time.TimetableId = id;
            db.MeetTime.Add(time);
            db.SaveChanges();
            return time;
        }
        public MeetTime ChangeMeetTime(MeetTime time, string userName)
        {
            time.WhoModified = userName;
            time.WhenModified = DateTime.Now;
            db.Entry(time).State = EntityState.Modified;
            db.SaveChanges();
            return time;
        }
        public void DeleteMeeTime(int id, string userName)
        {
            MeetTime time = db.MeetTime.Where(x => x.IsVisible && x.MeetTimeId == id).FirstOrDefault();
            time.IsVisible = false;
            time.WhoDeleted = userName;
            time.WhenDeleted = DateTime.Now;
            db.Entry(time).State = EntityState.Modified;
            db.SaveChanges();
        }
        #endregion

        #region Schedule
        public List<Schedule> WorkingJournalList(int id)
        {
            List<Schedule> list = db.Schedule.Where(x => x.IsVisible && x.DoctorId == id).OrderByDescending(x=>x.WhenAdded).ToList();
            return list;
        }

        public Schedule AddWorkingJournal(Schedule schedule, string userName, int id)
        {
            int? tabel = schedule.TimetableId;
            List<MeetTime> time = new List<MeetTime>();
            time = db.MeetTime.Where(x => x.TimetableId == tabel).ToList();
            for (int i = 0; i < time.Count(); i++)
            {
                DateTime date = schedule.Date;
                int year = date.Year;
                int month = date.Month;
                int day = date.Day;

                DateTime dateFrom = time[i].TimeFrom;
                int hourFrom = dateFrom.Hour;
                int minutesFrom = dateFrom.Minute;

                DateTime dateTo = time[i].TimeTo;
                int hourTo = dateTo.Hour;
                int minutesTo = dateTo.Minute;

                schedule.TimeFrom = new DateTime(year, month, day, hourFrom, minutesFrom, 0);
                schedule.TimeTo = new DateTime(year, month, day, hourTo, minutesTo, 0);
                schedule.IsVisible = true;
                schedule.WhoAdded = userName;
                schedule.WhenAdded = DateTime.Now;
                schedule.DoctorId = id;
                schedule.PacientId = null;
                schedule.isBooked = false;
                db.Schedule.Add(schedule);
                db.SaveChanges();

            }
            return schedule;
        }
        #endregion

        #region Registration
        public Schedule RegisterAppointment(Schedule schedule, int currentUser)
        {
            schedule.isBooked = true;
            schedule.PacientId = currentUser;
            db.Entry(schedule).State = EntityState.Modified;
            db.SaveChanges();
            return schedule;
        }
        #endregion

    }
}