using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebAPI.ActivityScheduler.Entities
{
    public class ActivityEntity
    {
        public ActivityEntity() { }
        public ActivityEntity(
            string name,
            string imageUrl,
            int itemQuantity,
            int minUserCount,
            int maxUserCount,
            string description,
            string location)
        {
            this.Name = name;
            this.ImageUrl = imageUrl;

            this.ItemQuantity = itemQuantity;
            this.MinUserCount = minUserCount;
            this.MaxUserCount = maxUserCount;

            this.Description = description;
            this.Location = location;
        }


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
