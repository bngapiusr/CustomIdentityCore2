using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CustomIdentityCore2.Web.Services;

namespace CustomIdentityCore2.Web.Models
{
    public class RoleViewModel
    {
        public int RoleId { get; set; }
        [Required]
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.")]
        public string Name { get; set; }
        public IEnumerable<MvcControllerInfo>SelectedControllers { get; set; }
    }
}