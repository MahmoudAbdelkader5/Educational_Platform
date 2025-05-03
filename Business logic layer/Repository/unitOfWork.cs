using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Business_logic_layer.Repository.unitOfWork;

namespace Business_logic_layer.Repository
{
    public class unitOfWork : IunitofWork,IDisposable
    {
        public IAssignmentRepo Assessment { get; set; }

        public IMessageRepo Message { get; set; }

        private readonly ApplicationDbContext dbcontext;
        public IExamRepo Exam { get; set; }

        public ICourseRepo Course { get;  set; }
        public ILessonRepo Lesson { get; set; }
        public IstudentRepo Student { get; set; }
        public IinstructorRepo Instructor { get; set; }
        public IRevisionRepo Revision { get; set; }
        public Istudent_CourseRepo student_CourseRepo { get; set; }
        public IquestionRepo questions { get; set; }
        public ICommentRepo Comment { get; set; }
        public IExamQuestionsRepo ExamQuestion { get; set; }
        public Istudent_answers student_answers { get; set; }
        public IStudent_Exam student_Exam { get; set; }
        public IAssignmentQuestionRepo AssignmentQuestion { get; set; }
        public Iassignment_AnswerRepo Iassignment_AnswerRepo { get; set; }
        public IStudent_AssignmentRepo Student_Assignment { get; set; }

        public unitOfWork(ApplicationDbContext dbcontext)
        {

            // 
            Student_Assignment = new Student_AssignmentRepo(dbcontext);
            Iassignment_AnswerRepo = new assignment_AnswerRepo(dbcontext);
            AssignmentQuestion = new AssignmentQuestionRepo(dbcontext);
            Assessment = new AssignmentRepo(dbcontext);
            ExamQuestion = new ExamQuestionsRepo(dbcontext);
            Message = new MessageRepo(dbcontext);
            Exam = new ExamRepo(dbcontext);
            questions = new QuestionRepo(dbcontext);
            Course = new CourseRepo(dbcontext);
            Lesson = new LessonRepo(dbcontext);
            Revision = new RevisionRepo(dbcontext);
            Student = new studentRepo(dbcontext);
            student_CourseRepo = new student_CourseRepo(dbcontext);
            Comment = new CommentRepo(dbcontext);
            Instructor = new InstructorRepo(dbcontext);
            student_answers = new student_answersRepo(dbcontext);
            student_Exam = new Student_ExamRepo(dbcontext);
            this.dbcontext = dbcontext;
        }

        public Task<int> Save()
        {
            return dbcontext.SaveChangesAsync();
        }

        //public async Task<int> SaveAsync()
        //{
        //    return await dbcontext.SaveChangesAsync();
        //}

        public void Dispose()
        {
            dbcontext.Dispose();
        }

        Task<int> IunitofWork.SaveAsync()
        {
            return dbcontext.SaveChangesAsync();
        }
    }
}