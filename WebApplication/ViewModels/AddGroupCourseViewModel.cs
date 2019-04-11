using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class AddGroupCourseViewModel
    {
        private List<Group> groups;

        public List<Group> Groups
        {
            get
            {
                if (groups == null) {
                    this.groups = new List<Group>();
                }
                return this.groups;
            }
            set
            {
                groups = value;
            }
        }

        private List<User> lecturers;

        public List<User> Lecturers
        {
            get
            {
                if (this.lecturers == null) {
                    this.lecturers = new List<User>();
                }
                return this.lecturers;
            }
            set
            {
                this.lecturers = value;
            }
        }

        private List<Course> courses;

        public List<Course> Courses
        {
            get
            {
                if (this.courses == null) {
                    this.courses = new List<Course>();
                }
                return this.courses;
            }
            set
            {
                this.courses = value;
            }
        }

        public GroupCourse GroupCourse { get; set; }

    }
}
