using backend_v3.Models.common;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_v3.Models
{
    public class YkienGopY : BaseModel
    {
        public string Id { get; set; }
        public string NoiDung { get; set; }
        public string? PhanHoi { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
