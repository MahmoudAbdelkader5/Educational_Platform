using Business_logic_layer.Repository;
using Data_access_layer.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_layer.interfaces
{
    public interface IStudent_Exam : IGenericRepo<Student_Exam>
    {
        Task<Student_Exam> GetFirstOrDefaultAsync(
            Expression<Func<Student_Exam, bool>> filter = null,
            Func<IQueryable<Student_Exam>, IOrderedQueryable<Student_Exam>> orderBy = null,
            string includeProperties = null,
            bool tracking = true);
    }
}
