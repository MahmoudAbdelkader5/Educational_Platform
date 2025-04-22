using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Business_logic_layer.Repository
{
    public class LessonRepo : genericRepo<Lesson>, ILessonRepo
    {
        private readonly ApplicationDbContext _context;

        public LessonRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public  async Task<int> GetCountAsync()
        {
            return await _context.Lessons.CountAsync();
        }

        
        public IQueryable<Lesson> searchCourseBytitle(string search)
        {
            return _context.Lessons
                .Where(l => l.Title.ToLower().StartsWith(search.ToLower()));
        }

      
    }
}