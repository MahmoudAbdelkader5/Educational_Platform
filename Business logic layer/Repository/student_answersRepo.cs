using Business_logic_layer.interfaces;
using Data_access_layer.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_layer.Repository
{
    public class student_answersRepo : genericRepo<student_answers>, Istudent_answers
    {
        private readonly ApplicationDbContext context;
        public student_answersRepo(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
      
    }
}
