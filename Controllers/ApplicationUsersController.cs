using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Data;
using BlogAPI.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ApplicationUsersController(ApplicationContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: api/ApplicationUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetApplicationUsers()
        {
          if (_context.ApplicationUsers == null)
          {
              return NotFound();
          }
            return await _context.ApplicationUsers.ToListAsync();
        }

        // GET: api/ApplicationUsers/5
        [HttpGet("{id}")]
        
        public async Task<ActionResult<ApplicationUser>> GetApplicationUser(string id)
        {
          if (_context.ApplicationUsers == null)
          {
              return NotFound();
          }
            var applicationUser = await _context.ApplicationUsers.FindAsync(id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            return applicationUser;
        }

        // PUT: api/ApplicationUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicationUser(string id, ApplicationUser applicationUser, string? currentPassword = null)
        {
            ApplicationUser applicationUser1 = _userManager.FindByIdAsync(applicationUser.Id).Result;

            if (id != applicationUser.Id)
            {
                return BadRequest();
            }

            string applicationUserid = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            if (applicationUserid != id)
            {
                return BadRequest();
            }

            applicationUser1.Address = applicationUser!.Address;
            applicationUser1.BirthDate = applicationUser!.BirthDate;
            applicationUser1.Email = applicationUser!.Email;
            applicationUser1.FirstName = applicationUser!.FirstName;
            applicationUser1.Gender = applicationUser!.Gender;
            applicationUser1.Id = applicationUser!.Id;
            applicationUser1.IdNumber = applicationUser!.IdNumber;
            applicationUser1.LastName = applicationUser!.LastName;
            applicationUser1.Password = applicationUser!.Password;
            applicationUser1.PhoneNumber = applicationUser!.PhoneNumber;
            applicationUser1.PostLikes = applicationUser!.PostLikes;
            applicationUser1.UserName = applicationUser!.UserName;
            applicationUser1.UserPosts = applicationUser!.UserPosts;

            _userManager.UpdateAsync(applicationUser1).Wait();

            if (currentPassword != null)
            {
                _userManager.ChangePasswordAsync(applicationUser1, currentPassword, applicationUser1.Password).Wait();
            }

            _context.Entry(applicationUser1).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(id))
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

        // POST: api/ApplicationUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> PostApplicationUser(ApplicationUser applicationUser)
        {
            if (_context.ApplicationUsers == null)
            {
                return Problem("Entity set 'ApplicationContext.ApplicationUsers'  is null.");
            }

            _userManager.CreateAsync(applicationUser, applicationUser.Password).Wait();

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApplicationUserExists(applicationUser.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApplicationUser", new { id = applicationUser.Id }, applicationUser);
        }

        // DELETE: api/ApplicationUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationUser(string id)
        {
            if (_context.ApplicationUsers == null)
            {
                return NotFound();
            }
            var applicationUser = await _context.ApplicationUsers.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            _context.ApplicationUsers.Remove(applicationUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApplicationUserExists(string id)
        {
            return (_context.ApplicationUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
