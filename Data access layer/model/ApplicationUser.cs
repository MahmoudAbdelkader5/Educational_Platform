using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_access_layer.model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public string? ProfilePicture { get; set; }


        // Foreign keys to link with domain models
        public int? StudentId { get; set; }
        public int? InstructorId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }

        [ForeignKey(nameof(InstructorId))]
        public virtual Instructor Instructor { get; set; }

        // Helper property to get the full name
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }

}
