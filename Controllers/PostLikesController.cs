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
using System.Security.Claims;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostLikesController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public PostLikesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/PostLikes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostLike>>> GetPostLikes()
        {
          if (_context.PostLikes == null)
          {
              return NotFound();
          }
            return await _context.PostLikes.ToListAsync();
        }

        // GET: api/PostLikes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostLike>> GetPostLike(int id)
        {
          if (_context.PostLikes == null)
          {
              return NotFound();
          }
            var postLike = await _context.PostLikes.FindAsync(id);

            if (postLike == null)
            {
                return NotFound();
            }

            return postLike;
        }

        // POST: api/PostLikes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostLike(int postId,string userId)
        {
            var post = await _context.Posts!.Include(p => p.PostLikes).FirstOrDefaultAsync(p => p.PostId == postId);

            if (post == null)
            {
                return NotFound();
            }

            if (post.PostLikes!.Any(l => l.UserId == userId))
            {
                return BadRequest("User has already liked this post.");
            }

            string applicationUserid = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            if (applicationUserid != userId)
            {
                return BadRequest();
            }

            var like = new PostLike { PostId = postId, UserId = userId };
            _context.PostLikes!.Add(like);

            post.LikeCount += 1;
            _context.Posts!.Update(post);
            await _context.SaveChangesAsync();

            return Ok(new { post.PostId, post.Title, post.Content, post.LikeCount });
        }

        // DELETE: api/PostLikes/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostLike(int id)
        {
            if (_context.PostLikes == null)
            {
                return NotFound();
            }
            var postLike = await _context.PostLikes.FindAsync(id);
            if (postLike == null)
            {
                return NotFound();
            }

            _context.PostLikes.Remove(postLike);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostLikeExists(int id)
        {
            return (_context.PostLikes?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
