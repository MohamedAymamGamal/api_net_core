using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.interfaces;
using api.Mappers;
using api.Models;
using api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.controller
{
    [Route("api/comments")]
    [ApiController]

    public class CommentController : ControllerBase
    {
        private readonly ICommentRepositiory _commentRepo;
        private readonly IstockRepository _stockRepo;
        public CommentController(ICommentRepositiory commentRepo, IstockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(s => s.ToCommentDto());
            return Ok(commentDto);

        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
              if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var comments = await _commentRepo.GetByIdAsync(id);

            if (comments == null)
            {
                return NotFound();
            }
            return Ok(comments.ToCommentDto());
        }
        [HttpPost("{stoctId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stoctId, CreateCommentDto commentDto)
        {
              if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await _stockRepo.StockExist(stoctId))
            {
                return BadRequest("Stock does not exist");
            }
            var commentModel = commentDto.ToCommentFromCreateDto(stoctId);
            await _commentRepo.CreateAsync(commentModel);
            return Ok();
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequest updateDto)
        {
              if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var commentModel = await _commentRepo.UpdateAsync(id, updateDto.ToCommentFromUpdate());
            if (commentModel == null)
            {
                return NotFound("comment not found");
            }

            return Ok(commentModel.ToCommentDto());
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
              if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var commentModel = await _commentRepo.DeleteAsync(id);
            if (commentModel == null)
            {
                return NotFound("Comment doesnt found");
            }
            return Ok();
        }
    }
}