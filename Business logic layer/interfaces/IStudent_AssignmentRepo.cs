using Data_access_layer.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_layer.interfaces
{
    public interface IStudent_AssignmentRepo : IGenericRepo<Student_Assignment>
    {
        Task<Student_Assignment> GetFirstOrDefaultAsync(
             Expression<Func<Student_Assignment, bool>> filter = null,
             string includeProperties = null,
             bool tracked = true);
       
    }
}
