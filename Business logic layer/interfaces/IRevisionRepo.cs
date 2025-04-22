using Data_access_layer.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_layer.interfaces
{
    public interface IRevisionRepo : IGenericRepo<Revision> 
    {
        public IQueryable<Revision> searchCourseBytitle(string search);
        Task<int> GetCountAsync();




    }
}
