using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Data;
using BlogAPI.Models;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersPostsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public UsersPostsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/UsersPosts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersPost>>> GetUsersPosts()
        {
          if (_context.UsersPosts == null)
          {
              return NotFound();
          }
            return await _context.UsersPosts.ToListAsync();
        }

        // GET: api/UsersPosts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsersPost>> GetUsersPost(int id)
        {
          if (_context.UsersPosts == null)
          {
              return NotFound();
          }
            var usersPost = await _context.UsersPosts.FindAsync(id);

            if (usersPost == null)
            {
                return NotFound();
            }

            return usersPost;
        }

        // PUT: api/UsersPosts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsersPost(int id, UsersPost usersPost)
        {
            if (id != usersPost.PostId)
            {
                return BadRequest();
            }

            _context.Entry(usersPost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersPostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UsersPosts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsersPost>> PostUsersPost(UsersPost usersPost)
        {
          if (_context.UsersPosts == null)
          {
              return Problem("Entity set 'ApplicationContext.UsersPosts'  is null.");
          }
            _context.UsersPosts.Add(usersPost);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UsersPostExists(usersPost.PostId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUsersPost", new { id = usersPost.PostId }, usersPost);
        }

        // DELETE: api/UsersPosts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsersPost(int id)
        {
            if (_context.UsersPosts == null)
            {
                return NotFound();
            }
            var usersPost = await _context.UsersPosts.FindAsync(id);
            if (usersPost == null)
            {
                return NotFound();
            }

            _context.UsersPosts.Remove(usersPost);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsersPostExists(int id)
        {
            return (_context.UsersPosts?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
