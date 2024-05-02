namespace backend_v3.Dto
{
    public class BlogDtos
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? AnhDaiDien { get; set; } = null!;
    }
}
