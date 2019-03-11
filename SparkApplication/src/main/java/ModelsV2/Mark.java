package ModelsV2;

public class Mark {

    private String courseId_;
    private int first_;
    private int second_;
    private int final_;
    private int n_;

    public Mark(String courseId,String first,String second,String n,String fin){
        first_ = Integer.parseInt(first);
        second_ = Integer.parseInt(second);
        final_ = Integer.parseInt(fin);
        n_ = Integer.parseInt(n);
        courseId_ = courseId;
    }

    public int getFinal_() {
        return final_;
    }

    public int getFirst_() {
        return first_;
    }

    public int getN_() {
        return n_;
    }

    public int getSecond_() {
        return second_;
    }

    public void setFinal_(int final_) {
        this.final_ = final_;
    }

    public void setFirst_(int first_) {
        this.first_ = first_;
    }

    public void setN_(int n_) {
        this.n_ = n_;
    }

    public void setSecond_(int second_) {
        this.second_ = second_;
    }

    public String getCourseId_() {
        return courseId_;
    }

    public void setCourseId_(String courseId_) {
        this.courseId_ = courseId_;
    }

    @Override
    public String toString() {
        return courseId_+","+first_+","+second_+","+final_+","+n_;
    }
}
