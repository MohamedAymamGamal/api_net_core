using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepositiory : ICommentRepositiory
    {
        private readonly ApplicationDBContext _context;
        public CommentRepositiory(ApplicationDBContext context )
        {
            _context = context;
        }

        public Task<Comment> CreateAsync(Stock stockModel)
        {
            throw new NotImplementedException();
        }

        public Task<Stock?> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }
        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FirstOrDefaultAsync(s => s.Id == id);
        }
        
    }
}