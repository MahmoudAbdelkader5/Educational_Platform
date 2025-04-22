using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_layer.Repository
{
   public  class studentRepo : genericRepo<Student>,IstudentRepo
    {
        private readonly ApplicationDbContext context;

        public studentRepo(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<int> GetCountAsync()
        {
            return await context.Students.CountAsync();
        }
    }
}
