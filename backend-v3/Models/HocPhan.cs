using System.ComponentModel.DataAnnotations.Schema;

namespace backend_v3.Models
{
    public class HocPhan
    {
        public string Id { get; set; }
        public string TieuDe { get; set; }
        public string MoTa { get; set; } = null!;
        public DateTime NgaySua { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
