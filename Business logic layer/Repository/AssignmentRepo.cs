using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_layer.Repository
{
    public class AssignmentRepo : genericRepo<Assignment>, IAssignmentRepo
    {
        public ApplicationDbContext Context { get; }

        public AssignmentRepo(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }
        public async Task<int> GetCountAsync()
        {
            return await Context.Assignments.CountAsync();
        }

        public async Task<Assignment> GetFirstOrDefaultAsync(Expression<Func<Assignment, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<Assignment> query = Context.Set<Assignment>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

       
    }
}
