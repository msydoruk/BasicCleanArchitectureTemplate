using AutoMapper;
using BasicCleanArchitectureTemplate.Core.Models;
using BasicCleanArchitectureTemplate.Infrastructure.Data.DataModels;
using BasicCleanArchitectureTemplate.Web.ViewModels;

namespace BasicCleanArchitectureTemplate.Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EventModel, EventDataModel>().ReverseMap();
            CreateMap<EventModel, EventViewModel>().ReverseMap();
            CreateMap<RecurrenceSettingModel, RecurrenceSettingDataModel>().ReverseMap();
            CreateMap<RecurrenceSettingModel, RecurrenceSettingViewModel>().ReverseMap();
        }
    }
}
