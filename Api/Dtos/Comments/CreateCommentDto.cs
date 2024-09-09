using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Comments
{
    public class CreateCommentDto
    {
        [Required]
        public int? StockId { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required, MaxLength(500)]
        public string Content { get; set; } = string.Empty;
    }
}
