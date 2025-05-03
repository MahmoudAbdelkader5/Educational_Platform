using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_access_layer.model
{
    public class PasswordResetToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        public DateTime ExpiryDate { get; set; }

        public bool IsUsed { get; set; }

        // Navigation property
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
