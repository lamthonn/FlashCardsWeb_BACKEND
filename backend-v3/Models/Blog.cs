using backend_v3.Models.common;

namespace backend_v3.Models
{
    public class Blog : BaseModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? AnhDaiDien { get; set; } = null!;
    }
}
