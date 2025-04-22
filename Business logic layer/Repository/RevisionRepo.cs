using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Business_logic_layer.Repository
{
    public class RevisionRepo : genericRepo<Revision>, IRevisionRepo
    {
        private readonly ApplicationDbContext _context;

        public RevisionRepo(ApplicationDbContext context) : base(context)
        {
            _context = context; // This assignment was missing
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Revisions.CountAsync();
        }

        public IQueryable<Revision> searchCourseBytitle(string search)
        {
            return _context.Revisions
                            .Where(r => r.Title.ToLower().StartsWith(search.ToLower()));
        }

       
    }
}