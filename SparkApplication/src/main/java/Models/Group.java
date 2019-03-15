package Models;

public class Group extends EntityBase {

    private String name;

    public Group(String id,String type,String name){
        super(id,type);
        this.name = name;
    }

    public Group(){
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
