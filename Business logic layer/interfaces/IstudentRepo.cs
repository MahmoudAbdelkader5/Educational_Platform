using Data_access_layer.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_layer.interfaces
{
   public  interface IstudentRepo : IGenericRepo<Student>
    {
        Task<int> GetCountAsync();
        void Update(Student student);
    }
}
