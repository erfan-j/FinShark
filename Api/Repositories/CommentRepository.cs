using Api.Data;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetListAsync()
        {
            var comments = await _context.Comments.AsQueryable()
                .OrderByDescending(x => x.CreationTime)
                .ToListAsync();
            return comments;
        }

        public async Task<Comment?> FindAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            return comment;
        }

        public async Task<Comment> CreateAsync(Comment Comment)
        {
            var newComment = await _context.Comments.AddAsync(Comment);
            await _context.SaveChangesAsync();
            return newComment.Entity;
        }

        public async Task<Comment?> UpdateAsync(int id, Comment comment)
        {
            var commentModel = await _context.Comments.FindAsync(id);
            if (commentModel is null) { return null; }

            commentModel.Title = comment.Title;
            commentModel.Content = comment.Content;

            await _context.SaveChangesAsync();

            return commentModel;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await _context.Comments.FindAsync(id);
            if (commentModel is null) { return null; }

            _context.Remove(commentModel);
            await _context.SaveChangesAsync();

            return commentModel;
        }
    }
}
