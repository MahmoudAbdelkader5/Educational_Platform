using Data_access_layer.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_layer.interfaces
{
    public interface IAssignmentQuestionRepo : IGenericRepo<assignment_question>
    {
        Task<int> GetCountAsync();

        public Task<IEnumerable<assignment_question>> GetQuestionsForExamAsync(int examId);

    }
}
