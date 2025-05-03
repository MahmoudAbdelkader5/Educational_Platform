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
        public IAssignmentRepo Assessment { get; set; }
        public IMessageRepo Message { get; set; }
        public IExamRepo Exam { get; set; }

        public IAssignmentQuestionRepo AssignmentQuestion { get; set; }
        public IExamQuestionsRepo ExamQuestion { get; set; }
        public Iassignment_AnswerRepo Iassignment_AnswerRepo { get; set; }
        public IquestionRepo questions { get; set; }
        public ICourseRepo Course { get;  set; }
       public ILessonRepo Lesson { get; set; }
        public IRevisionRepo Revision { get; set; }
        public IstudentRepo Student { get; set; }
        public IinstructorRepo Instructor { get; set; }
        public Istudent_CourseRepo student_CourseRepo { get; set; }
        public ICommentRepo Comment { get; set; }
        public Istudent_answers student_answers { get; set; }
        public IStudent_Exam student_Exam { get; set; }
        public IStudent_AssignmentRepo Student_Assignment { get; set; }
        Task<int> Save();
        Task<int> SaveAsync();
    }
}
