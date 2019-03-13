package ModelsV2;

import scala.Serializable;


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

    public int getFinal_() {
        return finalMark;
    }

    public int getFirst_() {
        return first;
    }

    public int getN_() {
        return n;
    }

    public int getSecond_() {
        return second;
    }

    public void setFinal_(int final_) {
        this.finalMark = final_;
    }

    public void setFirst_(int first_) {
        this.first = first_;
    }

    public void setN_(int n_) {
        this.n = n_;
    }

    public void setSecond_(int second_) {
        this.second = second_;
    }

    public String getCourseId_() {
        return courseId;
    }

    public void setCourseId_(String courseId_) {
        this.courseId = courseId_;
    }

    @Override
    public String toString() {
        return courseId+","+first+","+second+","+n+","+finalMark;
    }
}
