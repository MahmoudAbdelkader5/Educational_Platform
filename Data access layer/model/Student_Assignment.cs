using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_access_layer.model
{
    public class Student_Assignment
    {
        [Key]
        public int ID { get; set; }

        public int AssignmentID { get; set; }
        public int StudentID { get; set; }


        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Grade { get; set; }

        public string Feedback { get; set; }

        // Navigation properties
        [ForeignKey(nameof(AssignmentID))]
        public virtual Assignment Assignment { get; set; }

        [ForeignKey(nameof(StudentID))]
        public virtual Student Student { get; set; }
    }
}
