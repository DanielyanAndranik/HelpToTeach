package ModelsV2;

public class Course {

    private String id_;
    private String name_;

    public Course(){

    }

    public Course(String id,String name){
        this.name_ = name;
        this.id_ = id;
    }

    public String getId_() {
        return id_;
    }

    public String getName_() {
        return name_;
    }

    public void setId_(String id_) {
        this.id_ = id_;
    }

    public void setName_(String name_) {
        this.name_ = name_;
    }

    @Override
    public String toString() {
        return id_+","+name_;
    }
}
