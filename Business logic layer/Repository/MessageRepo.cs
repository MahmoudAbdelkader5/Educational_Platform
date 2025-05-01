using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Business_logic_layer.Repository
{
    public class MessageRepo : genericRepo<Message>, IMessageRepo
    {
        public ApplicationDbContext Context { get; }

        public async Task<int> GetCountAsync()
        {
            return await Context.Message.CountAsync();
        }
        public MessageRepo(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }
    }
}
