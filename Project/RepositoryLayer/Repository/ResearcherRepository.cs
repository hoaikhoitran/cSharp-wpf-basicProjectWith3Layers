using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repository
{
    public class ResearcherRepository
    {
        private Su25researchDbContext _context;

        public List<Researcher> GetAll()
        {
            _context = new();
            return _context.Researchers.ToList();
        }

    }
}
