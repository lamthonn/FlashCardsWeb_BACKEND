using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_v3.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public virtual User User{ get; set;} = null!;
    }
}
