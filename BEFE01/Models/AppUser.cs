using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BEFE01.Models
{
    public class AppUser : IdentityUser
    {
        [StringLength(200)]
        public required string FamilyName { get; set; } = "";

        [StringLength(200)]
        public required string GivenName { get; set; } = "";

        [StringLength(200)]
        public required string RefreshToken { get; set; } = "";
    }
}
