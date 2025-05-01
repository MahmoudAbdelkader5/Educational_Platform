using Business_logic_layer.interfaces;
using Data_access_layer.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_layer.Repository
{
    class Student_ExamRepo : genericRepo<Student_Exam>, IStudent_Exam
    {
        private readonly ApplicationDbContext context;
        public Student_ExamRepo(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
    
}
