using ActivityScheduler.Domain.Entities;
using ActivityScheduler.Presentation.EntitiesDTO;


namespace ActivityScheduler.Services.Extensions
{
    public static class CustomExtensions
    {
        public static void ApplyActivityEntityDetails(this ActivityResponseDTO activity, ActivityEntity activityEntity)
        {
            MapActivityEntityDetails(activity, activityEntity);
        }

        public static void ApplyActivityEntityDetails(this ActivityEntity activityEntity, ActivityEntityDTO activityEntityDTO)
        {
            MapActivityEntityDetails(activityEntity, activityEntityDTO);
        }

        private static void MapActivityEntityDetails(dynamic obj1, dynamic obj2)
        {
            obj1.Name = obj2?.Name;
            obj1.ImageUrl = obj2?.ImageUrl;
            obj1.ItemQuantity = obj2?.ItemQuantity;
            obj1.MinUserCount = obj2?.MinUserCount;
            obj1.MaxUserCount = obj2?.MaxUserCount;
            obj1.Description = obj2?.Description;
            obj1.Location = obj2?.Location;
        }
    }
}
