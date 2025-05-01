using Data_access_layer.model;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business_logic_layer.interfaces
{
    public interface IExamRepo : IGenericRepo<Exam>
    {
        Task<int> GetCountAsync();

        // Get an Exam by ID along with its questions  

        // Get first exam matching a condition  
        Task<Exam> GetFirstOrDefaultAsync(
            Expression<Func<Exam, bool>> filter = null,
            string includeProperties = null);
    }
}