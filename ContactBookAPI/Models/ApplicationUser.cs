using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace ContactBookAPI.Models
{
    public class ApplicationUser : IdentityUser 
    {
        public long Id { get; set; }

        public ICollection<ContactItem> ContactItem { get; set; }

    }
}
