using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business_logic_layer.Repository
{
    public class AssignmentQuestionRepo : genericRepo<assignment_question>, IAssignmentQuestionRepo
    {
        public ApplicationDbContext Context { get; }

        // Proper constructor syntax
        public AssignmentQuestionRepo(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        // Method to get questions for a specific exam
        public async Task<IEnumerable<assignment_question>> GetQuestionsForAssignmentAsync(int assignmentId)
        {
            return await Context.AssignmentQuestions
                .Where(aq => aq.AssignmentID == assignmentId)
                .Include(aq => aq.Question)  // Include related Question if needed
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await Context.AssignmentQuestions.CountAsync();
        }
    }
}