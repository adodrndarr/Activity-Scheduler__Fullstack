using ActivityScheduler.Domain.Entities;
using ActivityScheduler.Presentation.EntitiesDTO;
using AutoMapper;


namespace WebAPI.ActivityScheduler.Mappings
{
    public class ActivityProfile : Profile
    {
        public ActivityProfile()
        {
            CreateMap<Activity, ActivityRequestDTO>().ReverseMap();
            CreateMap<Activity, ActivityResponseDTO>().ReverseMap();

            CreateMap<ActivityEntity, ActivityEntityDTO>().ReverseMap();
            CreateMap<ActivityEntity, ActivityRequestDTO>().ReverseMap();

            CreateMap<Activity, BookedActivityDTO>();
        }
    }
}
