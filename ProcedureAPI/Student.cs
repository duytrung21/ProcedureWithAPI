using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProcedureAPI
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string StudentName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; } = String.Empty;
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

    }
}
