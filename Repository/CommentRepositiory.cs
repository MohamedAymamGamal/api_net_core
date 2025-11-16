using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepositiory : ICommentRepositiory
    {
        private readonly ApplicationDBContext _context;
        public CommentRepositiory(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment CommentModel)
        {
            await _context.Comments.AddAsync(CommentModel);
            await _context.SaveChangesAsync();
            return CommentModel;
        }

       

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(s => s.Id == id);
            if (commentModel == null)
            {
                return null;
            }
            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }


        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.Include(c => c.AppUser).ToListAsync();

        }
        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(s => s.Id == id);
        }

        // public async Task<Comment?> UpdateAsync(int id, UpdateCommentRequest commentDto)
        // {
            
        // }

        public async Task<Comment?> UpdateAsync(int id, Comment CommentModel)
        {
            var existingComment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (existingComment == null)
            {
                return null;
            }



            existingComment.Title = CommentModel.Title;
            existingComment.Content = CommentModel.Content;

            await _context.SaveChangesAsync();

            return existingComment;
        }
    }
}