using Business_logic_layer.interfaces;
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
        private readonly ApplicationDbContext dbcontext;
        public ICourseRepo Course { get;  set; }
        public ILessonRepo Lesson { get; set; }
        public IstudentRepo Student { get; set; }
        public IRevisionRepo Revision { get; set; }
        public Istudent_CourseRepo student_CourseRepo { get; set; }
        public unitOfWork(ApplicationDbContext dbcontext)
        {
            Course = new CourseRepo(dbcontext);
            Lesson = new LessonRepo(dbcontext);
            Revision = new RevisionRepo(dbcontext);
            Student = new studentRepo(dbcontext);
            student_CourseRepo = new student_CourseRepo(dbcontext);
            this.dbcontext = dbcontext;
        }

        public Task<int> Save()
        {
            return dbcontext.SaveChangesAsync();
        }

        public void Dispose()
        {
            dbcontext.Dispose();
        }

    }
}