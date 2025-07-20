using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repository
{
    //GUI UI <-> Service <-> Repository <-> Database <-> Table thực sự ở CSDL
    // L1          L2           l3
    // UI          BLL          DAL (CRUD Table)
    //Chuẩn hơn, thì thêm interface, class này implement interface
    public class ResearchProjectRepository
    {
        private Su25researchDbContext _context; //Khi dùng thì mới new

        //Hàm CRUD ứng với 4 lệnh  SQL cơ bản: Insert, Update, Delete, Select
        //Tên hàm gần với db, thô, ngắn gọn

        public List<ResearchProject> GetAll()
        {
            _context = new();
            /*return _context.ResearchProjects.ToList(); *///select * from ResearchProject
            return _context.ResearchProjects.Include("LeadResearcher").ToList();
        }

        public void Add(ResearchProject project)
        {
            _context = new();
            _context.ResearchProjects.Add(project); 
            _context.SaveChanges();
        }

        public void Update(ResearchProject project)
        {
            _context = new();
            _context.ResearchProjects.Update(project);
            _context.SaveChanges();
        }

        public void Delete(ResearchProject project) {
            _context = new();
            _context.ResearchProjects.Remove(project);
            _context.SaveChanges();
        }

    }
}
