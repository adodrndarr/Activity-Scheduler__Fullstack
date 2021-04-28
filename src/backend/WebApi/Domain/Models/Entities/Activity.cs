using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ActivityScheduler.Domain.Entities
{
    public class Activity
    {
        public Activity() 
        {
            this.DateBooked = DateTime.Now;
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public DateTime DateBooked { get; set; }
        public DateTime BookedForDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string ActivityEntityId { get; set; }
        public string UserId { get; set; }

        public string OrganizerName { get; set; }
        public string Duration { get; set; }
    }
}
