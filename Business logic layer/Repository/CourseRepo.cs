using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_layer.Repository
{
    public class CourseRepo : genericRepo<Course>, ICourseRepo
    {
        private readonly ApplicationDbContext context;

        public CourseRepo(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public IQueryable<Course> searchCourseBytitle(string search)
        {
            return context.Courses.Where(_context => _context.Title.ToLower().StartsWith(search));

        }
        public async Task<int> GetCountAsync()
        {
            return await context.Courses.CountAsync();
        }
    }
    
}
