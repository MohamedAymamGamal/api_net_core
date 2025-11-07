using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Mappers;
using api.Models;
using api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace api.controller
{
    [Route("api/comments")]
    [ApiController]

    public class CommentController : ControllerBase
    {
        private readonly ICommentRepositiory _commentRepo;
        public CommentController(ICommentRepositiory commentRepo)
        {
            _commentRepo = commentRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(s => s.ToCommentDto());
            return Ok(commentDto);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comments = await _commentRepo.GetByIdAsync(id);

            if (comments == null)
            {
                return NotFound();
            }
            return Ok(comments.ToCommentDto());
        }
    }
}