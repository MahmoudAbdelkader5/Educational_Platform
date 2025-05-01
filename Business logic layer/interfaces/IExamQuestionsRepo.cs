using Data_access_layer.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_layer.interfaces
{
    public interface IExamQuestionsRepo : IGenericRepo<examQuestion>
    {
        Task<int> GetCountAsync();

        public Task<IEnumerable<examQuestion>> GetQuestionsForExamAsync(int examId);
        
    }
}
