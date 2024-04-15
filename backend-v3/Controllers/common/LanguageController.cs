using backend_v3.Context;
using backend_v3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend_v3.Controllers.common
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly AppDbContext _context;
        public LanguageController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<NgonNgu>> GetAllLanguages()
        {
            try
            {
                var languages = await _context.NgonNgus.OrderBy(l => l.Name).ToListAsync();
                return languages;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
