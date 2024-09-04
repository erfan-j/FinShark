namespace Api.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int? StockId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; } = DateTime.Now;

        //navigation prop:
        public Stock? Stock { get; set; }
    }
}
