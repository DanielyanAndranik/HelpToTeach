package ModelsV2;

import java.io.Serializable;


public class Mark implements Serializable {


    private String courseId;
    private int first;
    private int second;
    private int finalMark;
    private int n;

    public Mark(String courseId,String first,String second,String n,String fin){
        this.first = Integer.parseInt(first);
        this.second = Integer.parseInt(second);
        finalMark = Integer.parseInt(fin);
        this.n = Integer.parseInt(n);
        this.courseId = courseId;
    }

    public int getFinal() {
        return finalMark;
    }

    public int getFirst() {
        return first;
    }

    public int getN() {
        return n;
    }

    public int getSecond() {
        return second;
    }

    public void setFinal(int final_) {
        this.finalMark = final_;
    }

    public void setFirst(int first_) {
        this.first = first_;
    }

    public void setN(int n_) {
        this.n = n_;
    }

    public void setSecond(int second_) {
        this.second = second_;
    }

    public String getCourseId() {
        return courseId;
    }

    public void setCourseId(String courseId_) {
        this.courseId = courseId_;
    }

    public boolean isValid(){
        return ((first + second + finalMark + n) > 40);
    }

    @Override
    public String toString() {
        return courseId+","+first+","+second+","+n+","+finalMark;
    }
}
