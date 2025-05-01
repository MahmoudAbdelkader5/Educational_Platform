using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_layer.Repository
{
    public class QuestionRepo :genericRepo<Questions>, IquestionRepo
    {
        private readonly ApplicationDbContext context;

        public QuestionRepo(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<int> GetCountAsync()
        {
            return await context.Questions.CountAsync();
        }
    }
    
}
