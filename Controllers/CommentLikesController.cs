using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Data;
using BlogAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentLikesController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public CommentLikesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/CommentLikes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentLike>>> GetCommentLikes()
        {
          if (_context.CommentLikes == null)
          {
              return NotFound();
          }
            return await _context.CommentLikes.ToListAsync();
        }

        // GET: api/CommentLikes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentLike>> GetCommentLike(long? id)
        {
          if (_context.CommentLikes == null)
          {
              return NotFound();
          }
            var commentLike = await _context.CommentLikes.FindAsync(id);

            if (commentLike == null)
            {
                return NotFound();
            }

            return commentLike;
        }


        // POST: api/CommentLikes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CommentLike(long commentId, string userId)
        {
            var comment = await _context.Comments!.Include(c => c.CommentLikes).FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                return NotFound();
            }

            string applicationUserid = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            if (applicationUserid != userId)
            {
                return BadRequest();
            }

            if (comment.CommentLikes!.Any(cl => cl.UserId == userId))
            {
                return BadRequest("User has already liked this comment.");
            }

            var like = new CommentLike { CommentId = commentId, UserId = userId };
            _context.CommentLikes!.Add(like);

            comment.LikeCount += 1;
            _context.Comments!.Update(comment);
            await _context.SaveChangesAsync();

            return Ok(new { comment.Id, comment.Content, comment.LikeCount });
        }

        // DELETE: api/CommentLikes/5
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> UnlikeComment(long commentId, string userId)
        {
            var comment = await _context.Comments!.Include(c => c.CommentLikes).FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                return NotFound();
            }

            string applicationUserid = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            if (applicationUserid != userId)
            {
                return BadRequest();
            }

            var like = comment.CommentLikes!.FirstOrDefault(cl => cl.UserId == userId);

            if (like == null)
            {
                return BadRequest("User has not liked this comment.");
            }

            _context.CommentLikes!.Remove(like);

            comment.LikeCount -= 1;

            await _context.SaveChangesAsync();

            return Ok(new { comment.Id, comment.Content, comment.LikeCount });
        }
    }
}
