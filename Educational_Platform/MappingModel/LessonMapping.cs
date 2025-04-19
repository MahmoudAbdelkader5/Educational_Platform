using AutoMapper;
using Data_access_layer.model;
using Educational_Platform.ViewModel;

namespace Educational_Platform.MappingModel
{
    public class LessonMapping : Profile
    {
        public LessonMapping()
        {
            CreateMap<Lesson, LessonViewModel>()
                .ForMember(dest => dest.CourseID, opt => opt.MapFrom(src => src.CourseID)); // Map CourseID
            CreateMap<LessonViewModel, Lesson>()
                .ForMember(dest => dest.CourseID, opt => opt.MapFrom(src => src.CourseID)); // Map CourseID
        }
    }
}

// ...existing code...
