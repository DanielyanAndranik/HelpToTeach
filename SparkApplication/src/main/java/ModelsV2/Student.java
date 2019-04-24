package ModelsV2;

import java.util.Date;
import java.util.List;

public class Student {

    private String id_;
    private String groupId_;
    private String firstName_;
    private String lastName_;
    private String middleName_;
    private Date birthDate_;
    private List<Mark> marks_;

    public Student(){

    }

    public String getId_() {
        return id_;
    }

    public List<Mark> getMarks_() {
        return marks_;
    }

    public String getFirstName_() {
        return firstName_;
    }

    public String getMiddleName_() {
        return middleName_;
    }

    public String getGroupId_() {
        return groupId_;
    }

    public String getLastName_() {
        return lastName_;
    }

    public Date getBirthDate_() {
        return birthDate_;
    }

    public void setId_(String id_) {
        this.id_ = id_;
    }

    public void setFirstName_(String firstName_) {
        this.firstName_ = firstName_;
    }

    public void setGroupId_(String groupId_) {
        this.groupId_ = groupId_;
    }

    public void setMiddleName_(String middleName_) {
        this.middleName_ = middleName_;
    }

    public void setLastName_(String lastName_) {
        this.lastName_ = lastName_;
    }

    public void setMarks_(List<Mark> marks_) {
        this.marks_ = marks_;
    }

    public void setBirthDate_(Date birthDate_) {
        this.birthDate_ = birthDate_;
    }

    public boolean isValid(){
        return marks_.size() == Info.COURSES_COUNT_2011 || marks_.size() == Info.COURSES_COUNT_2011_12;
    }

    @Override
    public boolean equals(Object other) {
        if (other == null) return false;
        if (other == this) return true;
        if (!(other instanceof Student))return false;
        Student otherMyClass = (Student) other;

        return this.hashCode() == otherMyClass.hashCode();

    }

    @Override
    public int hashCode() {
        return firstName_.hashCode() + groupId_.hashCode() + lastName_.hashCode() + middleName_.hashCode() + birthDate_.hashCode();
    }
}
