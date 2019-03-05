package Models;

public class Teacher extends EntityBase{

    private String firstName;
    private String lastName;

    public Teacher(String id,String type,String firstName,String lastName){
        super(id,type);
        this.firstName = firstName;
        this.lastName = lastName;
    }

    public Teacher(){
        super();
    }

    public String getLastName() {
        return lastName;
    }

    public String getFirstName() {
        return firstName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    @Override
    public String toString() {
        return getId() + "," + getType() + "," + firstName +","+lastName;
    }
}
