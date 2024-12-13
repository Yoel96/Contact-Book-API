using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactBookAPI.Context;
using ContactBookAPI.Models;
using System.Text.Json.Nodes;
using ContactBookAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
 
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.AspNetCore.Identity;

namespace ContactBookAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactItemsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ContactItemsController(AppDbContext context )
        {
            _context = context;

        }

        // GET: api/ContactItems
        // Gets all the contacts of the database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactItem>>> GetcontactItems()
        {
            return await _context.contactItems.ToListAsync();
        }

        // GET: api/ContactItems/5
        // Get one contact by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactItem>> GetContactItem(long id)
        {
            var contactItem = await _context.contactItems.FindAsync(id);

            if (contactItem == null)
            {
                return NotFound();
            }

            return contactItem;
        }

        // PUT: api/ContactItems/5
        // Updates the contact data
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactItem(long id, [FromBody] ContactItemDto dto)
        {
            // First we check if the user that was send is valid
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token");

            var newContact = new ContactItem
            {
                Id=id,
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Address = dto.Address,
                UserId = userId
            };

            _context.Entry(newContact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactItemExists(id))
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

        // GET: api/usersContacts
        // Gets the contacts associated with the user
        [HttpGet("usersContacts")]
        public async Task<ActionResult<ContactItem>> GetUserContacts()
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token");

            var userContacts = await _context.contactItems.Where(c => c.UserId == userId).Select(e => new {
                e.Id,
                e.Name,
                e.PhoneNumber,
                e.Email,
                e.Address,
            }).ToListAsync();
            return Ok( userContacts);

        }

        [HttpGet("ContactByPhone")]
        public async Task<ActionResult<ContactItem>> GetContactByPhone(string phone)
        {
            // First we check if the user that was send is valid
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token");

            var userContacts = await _context.contactItems.Where(c => EF.Functions.Like(c.PhoneNumber.ToString(), $"{phone}%")).ToListAsync();
            return Ok(userContacts);

        }

        [HttpGet("ContactByName")]
        public async Task<ActionResult<ContactItem>> GetContactByName(string name)
        {
            // First we check if the user that was send is valid
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token");

            var userContacts = await _context.contactItems.Where(c => EF.Functions.Like(c.Name, $"{name}%")).ToListAsync();
            return Ok(userContacts);

        }

        // POST: api/ContactItems
        // Create a new contact
        [HttpPost]
        public async Task<ActionResult<ContactItem>> PostContactItem([FromBody] ContactItemDto dto)
        {
            // First we check if the user that was send is valid
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token");

            var currentUser = await _context.Users.FindAsync(userId); ;

            var newContact = new ContactItem
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Email= dto.Email,
                Address= dto.Address,
                UserId = userId,
                User = currentUser,
            };

            _context.contactItems.Add(newContact);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetContactItem), new { id = newContact.Id }, newContact);
  
        }

        // DELETE: api/ContactItems/5
        // Delete a contact by id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactItem(long id)
        {
            // First we check if the contact exists
            var contactItem = await _context.contactItems.FindAsync(id);
            if (contactItem == null)
            {
                return NotFound();
            }

            _context.contactItems.Remove(contactItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

  

        private bool ContactItemExists(long id)
        {
            return _context.contactItems.Any(e => e.Id == id);
        }
    }
}
