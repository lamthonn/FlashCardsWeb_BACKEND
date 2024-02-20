namespace backend_v3.Dto
{
    public class AddUserRole
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Ten { get; set; }
        public List<int> RoleIds { get; set; }
    }
}
