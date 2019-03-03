namespace WebApi.Models
{
    public class GroupCourse:EntityBase<GroupCourse>
    {
        public string GroupId { get; set; }
        public string CourseId { get; set; }
        public string TeacherId { get; set; }
    }
}