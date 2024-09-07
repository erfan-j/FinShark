namespace Api.Dtos.Comments
{
    public class CreateCommentDto
    {
        public int StockId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
