using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomIdentityCore2.Entities
{
    public class UserRole
    {
        [Key,Required]
        public int UserRoleId { get; set; }
        [Required, ForeignKey(nameof(User))]
        public int UserId { get; set; }
        [Required, ForeignKey(nameof(Role))]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}