using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business_logic_layer.Repository
{
    class Student_ExamRepo : genericRepo<Student_Exam>, IStudent_Exam
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<Student_Exam> _dbSet;

        public Student_ExamRepo(ApplicationDbContext context) : base(context)
        {
            this.context = context;
            _dbSet = context.Set<Student_Exam>();
        }

        public async Task<Student_Exam> GetFirstOrDefaultAsync(
            Expression<Func<Student_Exam, bool>> filter = null,
            Func<IQueryable<Student_Exam>, IOrderedQueryable<Student_Exam>> orderBy = null,
            string includeProperties = null,
            bool tracking = true)
        {
            IQueryable<Student_Exam> query = _dbSet;

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