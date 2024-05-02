using backend_v3.Context;
using backend_v3.Dto;
using backend_v3.Interfaces;
using backend_v3.Models;
using Microsoft.EntityFrameworkCore;
namespace backend_v3.Services
{
    public class BlogService: IBlog
    {
        private readonly AppDbContext _context;

        public BlogService(AppDbContext context) { 
            _context = context;
        }

        public async Task<Blog> ThemBlog(BlogDtos data)
        {
            var newBlog = new Blog
            {
                Id = Guid.NewGuid().ToString(),
                Title = data.Title,
                Description = data.Description,
                Created = DateTime.Now,
                AnhDaiDien = data.AnhDaiDien,
            };
            _context.blogs.Add(newBlog);
            await _context.SaveChangesAsync(new CancellationToken());
            return newBlog;
        }

        public async Task<List<Blog>> GetAllBlog()
        {
            var data = _context.blogs.AsNoTracking();

            var result = await data.ToListAsync();
            return result;
        }

        public async Task EditBlog_Admin(string id, BlogDtos Blogdata)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("Không tìm thấy blog tương ứng");
                }

                var data = _context.blogs.FirstOrDefault(x => x.Id == id);

                if (data == null)
                {
                    throw new Exception("Blog không tồn tại!");
                }

                data.Title = Blogdata.Title;
                data.Description = Blogdata.Description;
                data.AnhDaiDien = Blogdata.AnhDaiDien;

                _context.blogs.Update(data);
                await _context.SaveChangesAsync(new CancellationToken());

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteBlog_Admin(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("Không tìm thấy id Blog");
            }

            var data = _context.blogs.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                throw new Exception("Blog không tồn tại!");
            }

            _context.blogs.Remove(data);

            await _context.SaveChangesAsync(new CancellationToken());
        }


    }
}
