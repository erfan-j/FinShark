using Api.Dtos.Comments;
using Api.Interfaces;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("/api/comment")]
    [ApiController]
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IMapper _mapper;



        public CommentsController(ICommentRepository commentRepository, IMapper mapper, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepository.GetListAsync();

            var commentsDto = _mapper.Map<List<Comment>, List<CommentDto>>(comments);
            return Ok(commentsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var comment = await _commentRepository.FindAsync(id);
            if (comment is null) { return NotFound(); }
            var commentDto = _mapper.Map<Comment>(comment);
            return Ok(commentDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Create(CreateCommentDto input) 
        {
            if (!await _stockRepository.CheckExistAsync(input.StockId)) { return BadRequest("Stock not found"); }
            var comment = _mapper.Map<Comment>(input);
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            comment.UserId = userId;
            var newComment = await _commentRepository.CreateAsync(comment);
            var commentDto = _mapper.Map<Comment, CommentDto>(newComment);
            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, commentDto);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto input)
        {
            var comment = _mapper.Map<UpdateCommentDto, Comment>(input);
            var updatedComment = await _commentRepository.UpdateAsync(id, comment);
            if (updatedComment is null) { return NotFound(); }
            var commentDto = _mapper.Map<Comment, CommentDto>(updatedComment);
            return Ok(commentDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedComment = await _commentRepository.DeleteAsync(id);
            if (deletedComment is null) { return NotFound(); }
            return Ok();
        }
    }
}
