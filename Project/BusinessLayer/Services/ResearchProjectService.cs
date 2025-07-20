using Microsoft.IdentityModel.Tokens;
using RepositoryLayer;
using RepositoryLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class ResearchProjectService
    {
        private ResearchProjectRepository _repo = new(); //Khi dùng thì mới new, không cần new ở đây, vì sẽ dùng hàm GetAllResearchProjects() để lấy dữ liệu
        //Hàm CRUD++, Tên hàm đặc dể hiểu, gần hươn với user
        public List<ResearchProject> GetAllResearchProjects()
        {
            return _repo.GetAll();
        }

        //GUI UI phải gửi x cho hàm này
        public void AddResearchProject(ResearchProject x)
        {
            _repo.Add(x);
        }

        public void UpdateResearchProject(ResearchProject x)
        {
            _repo.Update(x);
        }

        public void DeleteResearchProject(ResearchProject x)
        {
            _repo.Delete(x);
        }

        //Hàm search project theo title,field tùy theo yêu cầu để bài
        //nếu search số thì bắt ngoại lệ = bool TryParse, nếu search chuỗi thì dùng Contains
        //  int q; bool isNumber = int.TryParse(textBox, out q); true là convert được để tìm, false là nhập sai k convert đc
        // có thể dùng biến.HasValue để kiểm tra nullable, nếu có giá trị thì true, không có giá trị thì false cho số 
        //nếu cho search 2 ô nhập riêng biệt thì có 3 trường hợp để search
        public List<ResearchProject> SearchProjectsByTitleAndField(string? keyword)
        {
            //1. k ghõ keyword thì trả về tất cả
            List<ResearchProject> result = _repo.GetAll();
            if (keyword.IsNullOrEmpty())
                return result;
            //2. nếu có ghõ keyword thì tìm kiếm theo title và field
            if(!keyword.IsNullOrEmpty())
                result = result.Where(xxx => xxx.ProjectTitle.ToLower().Contains(keyword.ToLower()) 
                                                || xxx.ResearchField.ToLower().Contains(keyword.ToLower())).ToList(); 
            return result; 
        }

    }
}
