package Models;

public class GroupCourse extends EntityBase{
    public String groupId;
    public String courseId;
    public String teacherId;

    public GroupCourse(){
        super();
    }

    public String getGroupId() {
        return groupId;
    }

    public String getCourseId() {
        return courseId;
    }

    public String getTeacherId() {
        return teacherId;
    }

    public void setGroupId(String groupId) {
        this.groupId = groupId;
    }

    public void setCourseId(String courseId) {
        this.courseId = courseId;
    }

    public void setTeacherId(String teacherId) {
        this.teacherId = teacherId;
    }
}
