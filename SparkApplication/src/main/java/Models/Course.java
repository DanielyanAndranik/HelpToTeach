package Models;

public class Course extends  EntityBase{
    private String name;

    public Course(){
        super();
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    @Override
    public String toString() {
        return getId() + "," + getType() + "," + name;
    }
}
