using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business_logic_layer.Repository
{
    public class ExamRepo : genericRepo<Exam>, IExamRepo
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<Exam> _dbSet; // Added declaration for _dbSet

        public ExamRepo(ApplicationDbContext context) : base(context)
        {
            this.context = context;
            this._dbSet = context.Set<Exam>(); // Initialize _dbSet
        }

        public async Task<int> GetCountAsync()
        {
            return await context.Exams.CountAsync();
        }

        public async Task<Exam> GetFirstOrDefaultAsync(
            Expression<Func<Exam, bool>> filter = null,
            string includeProperties = null)
        {
            IQueryable<Exam> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}