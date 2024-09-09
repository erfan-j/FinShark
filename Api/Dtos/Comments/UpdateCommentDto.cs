using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Comments
{
    public class UpdateCommentDto
    {
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string Content { get; set; } = string.Empty;
    }
}
