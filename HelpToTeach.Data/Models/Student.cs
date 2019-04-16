using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpToTeach.Data.Models
{
    public class Student : EntityBase<Student>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        public bool FullScholarship { get; set; }
        public string GroupId { get; set; }
        public Group Group { get; set; }
    }
}
