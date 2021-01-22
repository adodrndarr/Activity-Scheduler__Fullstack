using System;
using System.ComponentModel.DataAnnotations;


namespace WebAPI.ActivityScheduler.EntitiesDTO
{
    public class ActivityEntityDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Item quantity is required.")]
        public int ItemQuantity { get; set; }

        [Required(ErrorMessage = "Minimum user count is required.")]
        public int MinUserCount { get; set; }

        [Required(ErrorMessage = "Maximum user count is required.")]
        public int MaxUserCount { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; }
    }
}
