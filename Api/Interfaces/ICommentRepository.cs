using Api.Models;

namespace Api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetListAsync();
        Task<Comment?> FindAsync(int id);
        Task<Comment> CreateAsync(Comment Comment);
        Task<Comment?> UpdateAsync(int id, Comment Comment);
        Task<Comment?> DeleteAsync(int id);
    }
}
