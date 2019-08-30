using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace CustomIdentityCore2.Entities
{
    public class User
    {
        public User()
        {
            UserRole = new List<UserRole>();
        }
        [Key, Required]
        public int  UserId { get; set; }
        [Required, MaxLength(128)]
        public string UserName { get; set; }
        [Required, MaxLength(1024)]
        public string PasswordHash { get; set; }
        [Required, MaxLength(128)]
        public string Email { get; set; }
        [Required, MaxLength(128)]
        public bool EmailConfirmed { get; set; }
        [MaxLength(32)]
        public string FirstName { get; set; }
        [MaxLength(1)]
        public string MiddleInitial { get; set; }
        [MaxLength(32)]
        public string  LastName { get; set; }
        public virtual ICollection<UserRole> UserRole{ get; set; }
    }
}