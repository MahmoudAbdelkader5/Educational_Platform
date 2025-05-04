using Data_access_layer.model;
using System;
using System.Collections.Generic;

namespace Educational_Platform.ViewModel
{
    public class CourseDetailsViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string Image { get; set; }
        public List<LessonViewModel> Lessons { get; set; }
        public List<CommentViewModel> Comments { get; set; }

        public List<Student_Assignment> Assignments { get; set; }
        // Add this property
        public string CurrentProfilePicture { get; set; }
    }
}
