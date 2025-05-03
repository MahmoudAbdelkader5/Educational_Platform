using Data_access_layer.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_layer.interfaces
{
    public interface IAssignmentRepo : IGenericRepo<Assignment>
    {
        Task<int> GetCountAsync();

        // Get an Exam by ID along with its questions  

        // Get first exam matching a condition  
        Task<Assignment> GetFirstOrDefaultAsync(
            Expression<Func<Assignment, bool>> filter = null,
            string includeProperties = null);
    }
}
