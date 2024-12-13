using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactBookAPI.Models
{
    public class ContactItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

    }
}
