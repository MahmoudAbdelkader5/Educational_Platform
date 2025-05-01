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
    public class student_CourseRepo : genericRepo<student_Course>, Istudent_CourseRepo
    {
        private readonly ApplicationDbContext context;

        public student_CourseRepo(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<int> GetCountAsync()
        {
            return await context.Enrollments.CountAsync();
        }
    }
}
