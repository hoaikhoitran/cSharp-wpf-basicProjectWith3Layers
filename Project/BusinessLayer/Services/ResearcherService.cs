using RepositoryLayer;
using RepositoryLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class ResearcherService
    {
        private ResearcherRepository _repository = new();
        public List<Researcher> GetAllResearchers()
        {
            return _repository.GetAll();
        }
    }
}
