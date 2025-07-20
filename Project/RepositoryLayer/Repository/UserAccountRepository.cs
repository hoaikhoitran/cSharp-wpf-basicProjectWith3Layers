using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repository
{
    public class UserAccountRepository
    {
        private Su25researchDbContext _context;  //ko new, mỗi lần sài mới new()

        public UserAccount GetOne(string email, string password)
        {
            _context = new();
            return _context.UserAccounts.FirstOrDefault(x => x.Email.ToLower() == email.ToLower() && x.Password == password);
        }//hoặc trả về 1 dòng, hoặc trả về null nếu không tìm thấy email và password, sai 1 trong 2
    }
}
