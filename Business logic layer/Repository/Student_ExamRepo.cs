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
    class Student_ExamRepo : genericRepo<Student_Exam>, IStudent_Exam
    {
        private readonly ApplicationDbContext context;
        public Student_ExamRepo(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }


        public async Task<Student_Exam> GetFirstOrDefaultAsync(
                  Expression<Func<Student_Exam, bool>> filter = null,
                  string includeProperties = null,
                  bool tracked = true)
        {
            IQueryable<Student_Exam> query;

            if (tracked)
            {
                query = context.Set<Student_Exam>();
            }
            else
            {
                query = context.Set<Student_Exam>().AsNoTracking();
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
