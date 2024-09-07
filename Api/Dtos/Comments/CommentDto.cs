namespace Api.Dtos.Comments
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int? StockId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; }
    }
}
