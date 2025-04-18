using AutoMapper;
using Data_access_layer.model;
using Educational_Platform.ViewModel;

namespace Educational_Platform.MappingModel
{
    public class mappingCourse : Profile
    {
        public mappingCourse()
        {
            CreateMap<Course, CourseViewModel>().ReverseMap();
        }

    }
}
