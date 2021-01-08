using System;
using WebAPI.ActivityScheduler.Entities;
using WebAPI.ActivityScheduler.EntitiesDTO.ManageUsers;


namespace WebAPI.ActivityScheduler.EntitiesDTO
{
    public class ActivityDTO
    {
        public Guid Id { get; set; }

        public DateTime DateBooked { get; set; }
        public DateTime BookedForDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public ActivityEntity ActivityEntity { get; set; }
        public UserDTO User { get; set; }

        public string OrganizerName { get; set; }
        public string Duration { get; set; }
    }
}