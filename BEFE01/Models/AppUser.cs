using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BEFE01.Models
{
    public class AppUser : IdentityUser
    {
        [StringLength(200)]
        public string FamilyName { get; set; } = string.Empty;

        [StringLength(200)]
        public string GivenName { get; set; } = string.Empty;

        [StringLength(200)]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
