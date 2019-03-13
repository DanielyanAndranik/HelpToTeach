using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class AddStudentViewModel
    {
        private Student student;
        public Student Student
        {
            get
            {
                if (this.student == null)
                    this.student = new Student();
                return this.student;
            }
            set
            {
                this.student = value;
            }
        }
        private IEnumerable<Group> groups;
        public IEnumerable<Group> Groups
        {
            get
            {
                if (this.groups == null)
                    this.groups = new List<Group>();
                return this.groups;
            }
            set
            {
                this.groups = value;
            }
        }

    }
}
