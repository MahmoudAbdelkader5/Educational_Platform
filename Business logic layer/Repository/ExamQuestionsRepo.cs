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
    public class ExamQuestionsRepo : genericRepo<examQuestion>, IExamQuestionsRepo
    {
        private readonly ApplicationDbContext context;

        public ExamQuestionsRepo(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<examQuestion>> GetQuestionsForExamAsync(int examId)
        {
            return await context.ExamQuestions
                .Include(eq => eq.Question)  // Eager load the Question navigation property
                .Where(eq => eq.ExamID == examId)
                .ToListAsync();
        }
        public async Task<int> GetCountAsync()
        {
            return await context.ExamQuestions.CountAsync();
        }

    }

}
