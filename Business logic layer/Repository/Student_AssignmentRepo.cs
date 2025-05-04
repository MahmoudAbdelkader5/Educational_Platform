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
        private readonly ApplicationDbContext _context;

        public Student_AssignmentRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Student_Assignment> GetFirstOrDefaultAsync(
            Expression<Func<Student_Assignment, bool>> filter = null,
            string includeProperties = null,
            bool tracked = true)
        {
            IQueryable<Student_Assignment> query;

            if (tracked)
            {
                query = _context.Set<Student_Assignment>();
            }
            else
            {
                query = _context.Set<Student_Assignment>().AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(
                    new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            return await query.FirstOrDefaultAsync();
        }

    }
}