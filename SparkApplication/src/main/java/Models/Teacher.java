package Models;

public class Teacher {

    private String id;
    private String firstName;
    private String lastName;

    public Teacher(){

    }

    public Teacher(String id,String fn,String ln){
        this.id = id;
        this.firstName = fn;
        this.lastName = ln;
    }

    public String getId(){
        return this.id;
    }

    public String getFirstName() {
        return firstName;
    }

    public String getLastName() {
        return lastName;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public void setId(String id) {
        this.id = id;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }
}
