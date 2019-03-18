using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Student:EntityBase<Student>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Mark> Marks { get; set; }
        //public string MiddleName { get; set; }
        public string GroupId { get; set; }
    }
}
