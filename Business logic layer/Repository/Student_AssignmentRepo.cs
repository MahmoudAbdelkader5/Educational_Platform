using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business_logic_layer.Repository
{
  public  class Student_AssignmentRepo : genericRepo<Student_Assignment>, IStudent_AssignmentRepo
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<Student_Assignment> _dbSet;

        public Student_AssignmentRepo(ApplicationDbContext context) : base(context)
        {
            this.context = context;
            _dbSet = context.Set<Student_Assignment>();
        }

        public async Task<Student_Assignment> GetFirstOrDefaultAsync(
            Expression<Func<Student_Assignment, bool>> filter = null,
            Func<IQueryable<Student_Assignment>, IOrderedQueryable<Student_Assignment>> orderBy = null,
            string includeProperties = null,
            bool tracking = true)
        {
            IQueryable<Student_Assignment> query = _dbSet;

            if (!tracking)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.FirstOrDefaultAsync();
        }

       
    }
}