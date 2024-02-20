using System.ComponentModel.DataAnnotations;

namespace backend_v3.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
