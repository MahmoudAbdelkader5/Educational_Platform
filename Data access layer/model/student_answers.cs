using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_access_layer.model
{
    public class student_answers
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey(nameof(examQuestion))]
        public int examQuestionID { get; set; }

        [ForeignKey(nameof(Student))]
        public int StudentID { get; set; }

        //public int Totalgrade { get; set; } = 0;
        public string AnswerText { get; set; }

        // Navigation properties
        public virtual examQuestion examQuestion { get; set; }
        public virtual Student Student { get; set; }
    }


}
