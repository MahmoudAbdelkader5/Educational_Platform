using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_access_layer.model;
namespace Business_logic_layer.interfaces
{
    public interface ILessonRepo : IGenericRepo<Lesson>
    {
        public IQueryable<Lesson> searchCourseBytitle(string search);
        Task<int> GetCountAsync();


    }
}
