package Models;

public class Student extends EntityBase {

    private String firstName;
    private String lastName;
    private String groupId;

    public Student(){
        super();
    }

    public String getFirstName() {
        return firstName;
    }

    public String getLastName() {
        return lastName;
    }

    public String getGroupId() {
        return groupId;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    public void setGroupId(String groupId) {
        this.groupId = groupId;
    }

    @Override
    public String toString() {
        return getId()+","+getType()+","+getFirstName()+","+getLastName()+","+getGroupId();
    }
}
