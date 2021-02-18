using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ActivityScheduler.Domain.Entities
{
    public class ActivityEntity
    {
        public ActivityEntity() { }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public int ItemQuantity { get; set; }

        public int MinUserCount { get; set; }
        public int MaxUserCount { get; set; }

        public string Description { get; set; }
        public string Location { get; set; }
    }
}
