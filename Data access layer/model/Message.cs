using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_access_layer.model
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }
        [StringLength(255)]
        [Required]
        public string Name { get; set; }
        [StringLength(255)]
        public string? Subject { get; set; }
        [Required]

        [StringLength(4000)]
        public string MessageBody { get; set; }
        [StringLength(255)]
        [Required]
        public string Email { get; set; }
    }
}
