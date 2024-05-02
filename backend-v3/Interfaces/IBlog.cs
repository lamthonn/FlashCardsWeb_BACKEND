using backend_v3.Dto;
using backend_v3.Dto.Common;
using backend_v3.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend_v3.Interfaces
{
    public interface IBlog
    {
        public Task<Blog> ThemBlog(BlogDtos data);
        public Task<List<Blog>> GetAllBlog();
        public Task EditBlog_Admin(string id, BlogDtos Blogdata);
        public Task DeleteBlog_Admin(string id);
    }
}
