using backend_v3.Context;
using backend_v3.Controllers.common;
using backend_v3.Dto;
using backend_v3.Dto.Common;
using backend_v3.Interfaces;
using backend_v3.Models;
using backend_v3.Seriloger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace backend_v3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BlogController: ControllerBase
    {
        private IBlog _service;
        private readonly IWebHostEnvironment _environment;
        private readonly AppDbContext _context;

        private readonly LoggingCommon _loggingCommon;

        public BlogController(IBlog service, AppDbContext context, IWebHostEnvironment environment)
        {
            _service = service;
            _context = context;
            _environment = environment;

        }
        [HttpGet]
        public Task<List<Blog>> GetAllBlog()
        {

            try
            {
                var res = _service.GetAllBlog();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<Blog> ThemBlog(BlogDtos data)
        {
            try
            {
                var res = await _service.ThemBlog(data);
                return res;
            }
            catch (Exception ex)
            {           
                throw new Exception(ex.Message);
            }
        }

        [HttpPut]
        public async Task<bool> EditBlog_Admin(string id, BlogDtos Blogdata)
        {
            try
            {
                await _service.EditBlog_Admin(id, Blogdata);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpDelete]
        public async Task<bool> DeleteBlog_Admin(string id)
        {
            try
            {
                await _service.DeleteBlog_Admin(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetFile(string id)
        {
            var path = _context.blogs.FirstOrDefault(x => x.Id == id)!.AnhDaiDien;
            try
            {
                var pathFile = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/", path);

                if (System.IO.File.Exists(pathFile))
                {
                    FileInfo fileInfo = new FileInfo(pathFile);
                    var realFilePath = fileInfo.FullName;
                    //chong doc ra ngoai thu muc
                    if (realFilePath.IndexOf("imageBlog") < 0) return NotFound();
                    //var fileName = System.IO.Path.GetFileName(pathFile);
                    string fileName = fileInfo.Name;
                    //string fileExtension = fileInfo.Extension;
                    new FileExtensionContentTypeProvider()
                                    .TryGetContentType(fileName, out string contentType);
                    var fileContent = await System.IO.File.ReadAllBytesAsync(pathFile);

                    return File(fileContent, contentType, fileName);
                }

                return NotFound(); // returns a NotFoundResult with Status404NotFound response.
            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
                return Problem(detail: ioExp.Message);
            }
        }

        [HttpPost]
        public async Task<string> UploadFileAsync(FileUploadBytes fileUpload)
        {
            if (fileUpload == null)
            {
                throw new ArgumentNullException(nameof(fileUpload));
            }

            if (string.IsNullOrWhiteSpace(fileUpload.fileName))
            {
                throw new ArgumentException("File name is missing or empty.", nameof(fileUpload));
            }

            if (string.IsNullOrWhiteSpace(fileUpload.contentType))
            {
                throw new ArgumentException("File content type is missing or empty.", nameof(fileUpload));
            }

            try
            {
                // Construct the path to the avatar directory under wwwroot
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "imageBlog");

                // Check if the avatar directory exists, create it if not
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Giải mã dữ liệu base64 thành byte array
                byte[] imageBytes = Convert.FromBase64String(fileUpload.contentBase64);

                // Lưu ảnh vào thư mục wwwroot/avatar
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + fileUpload.fileName;
                string filePath = Path.Combine(_environment.WebRootPath, "imageBlog", uniqueFileName);

                await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);
                //await File.WriteAllBytesAsync(filePath, fileUpload.content);

                // Tạo đường dẫn ảnh lưu trữ
                string imageUrl = $"imageBlog/{uniqueFileName}";

                // Trả về đường dẫn ảnh lưu trữ hoặc base64 string (tùy chọn)
                return imageUrl;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while uploading file.", ex);
            }
        }

    }
}
