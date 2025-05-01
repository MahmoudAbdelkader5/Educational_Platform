using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_access_layer.model;
using Microsoft.EntityFrameworkCore;

namespace Business_logic_layer.interfaces
{
    public interface IMessageRepo : IGenericRepo<Message>
    {
        Task<int> GetCountAsync();

    }
}
