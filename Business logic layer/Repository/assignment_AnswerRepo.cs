using Business_logic_layer.interfaces;
using Data_access_layer.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_layer.Repository
{
    public class assignment_AnswerRepo : genericRepo<assignment_Answer>, Iassignment_AnswerRepo
    {
        private readonly ApplicationDbContext context;
        public assignment_AnswerRepo(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

    }
}
