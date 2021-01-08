using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebAPI.ActivityScheduler.Entities
{
    public class Activity
    {
        public Activity() 
        {
            this.DateBooked = DateTime.Now;
        }
        public Activity(
            string name,
            string imageUrl,
            DateTime bookedForDate,
            DateTime startTime,
            DateTime endTime,
            int itemQuantity,
            int minUserCount,
            int maxUserCount,
            string description,
            string location,
            string organizerName)
        {
            this.ActivityEntity.Name = name;
            this.ActivityEntity.ImageUrl = imageUrl;

            this.DateBooked = DateTime.Now;
            this.BookedForDate = bookedForDate;
            this.StartTime = startTime;
            this.EndTime = endTime;
            
            this.ActivityEntity.ItemQuantity = itemQuantity;
            this.ActivityEntity.MinUserCount = minUserCount;
            this.ActivityEntity.MaxUserCount = maxUserCount;
            
            this.ActivityEntity.Description = description;
            this.ActivityEntity.Location = location;
            this.OrganizerName = organizerName;
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public DateTime DateBooked { get; set; }
        public DateTime BookedForDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public ActivityEntity ActivityEntity { get; set; }
        public User User { get; set; }
        
        public string OrganizerName { get; set; }
        public string Duration { get; set; }
    }
}
