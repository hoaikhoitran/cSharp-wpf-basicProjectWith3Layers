using RepositoryLayer;
using RepositoryLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserAccountService
    {
        private UserAccountRepository _repo = new();
        
        public UserAccount? Authenticate(string email, string password)
        {
            return _repo.GetOne(email, password);
        }
    }
}
