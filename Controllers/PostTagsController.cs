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

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostTagsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public PostTagsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/PostTags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostTag>>> GetPostTags()
        {
          if (_context.PostTags == null)
          {
              return NotFound();
          }
            return await _context.PostTags.ToListAsync();
        }

        // GET: api/PostTags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostTag>> GetPostTag(int id)
        {
          if (_context.PostTags == null)
          {
              return NotFound();
          }
            var postTag = await _context.PostTags.FindAsync(id);

            if (postTag == null)
            {
                return NotFound();
            }

            return postTag;
        }

        // POST: api/PostTags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PostTag>> PostPostTag(PostTag postTag)
        {
          if (_context.PostTags == null)
          {
              return Problem("Entity set 'ApplicationContext.PostTags'  is null.");
          }

            _context.PostTags.Add(postTag);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PostTagExists(postTag.PostId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPostTag", new { id = postTag.PostId }, postTag);
        }

        // DELETE: api/PostTags/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostTag(int id)
        {
            if (_context.PostTags == null)
            {
                return NotFound();
            }
            var postTag = await _context.PostTags.FindAsync(id);
            if (postTag == null)
            {
                return NotFound();
            }

            _context.PostTags.Remove(postTag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostTagExists(int id)
        {
            return (_context.PostTags?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
