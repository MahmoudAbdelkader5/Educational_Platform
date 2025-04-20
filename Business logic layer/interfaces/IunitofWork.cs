using Data_access_layer.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_layer.interfaces
{
   public interface IunitofWork
    {
        public ICourseRepo Course { get;  set; }
       public ILessonRepo Lesson { get; set; }
        public IRevisionRepo Revision { get; set; }
        public IstudentRepo Student { get; set; }
        public Istudent_CourseRepo student_CourseRepo { get; set; }
        Task<int> Save();
    }
}
