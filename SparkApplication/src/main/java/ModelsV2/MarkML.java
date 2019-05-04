package ModelsV2;

import java.io.Serializable;

public class MarkML implements Serializable {

    private String id_;
    private int markType_;
    private boolean absent_;
    private int value_;
    private String studentId_;
    private boolean isPredicted_;

    public static final int FIRST_MIDDLE = 2;
    public static final int ACTIVITY = 0;
    public static final int SECOND_MIDDLE = 3;
    public static final int FINAL = 4;



    public MarkML(String id,int type,String studentId,int value){
        id_ = id;
        markType_ = type;
        studentId_ = studentId;
        absent_ = false;
        isPredicted_ = false;
        value_ = value;
    }


    public int getMarkType_() {
        return markType_;
    }

    public int getValue_() {
        return value_;
    }

    public String getStudentId_() {
        return studentId_;
    }

    public boolean isAbsent_() {
        return absent_;
    }

    public boolean isPredicted_() {
        return isPredicted_;
    }

    public void setAbsent_(boolean absent_) {
        this.absent_ = absent_;
    }

    public void setMarkType_(int markType_) {
        this.markType_ = markType_;
    }

    public void setPredicted_(boolean predicted_) {
        isPredicted_ = predicted_;
    }

    public void setStudentId_(String studentId_) {
        this.studentId_ = studentId_;
    }

    public void setValue_(int value_) {
        this.value_ = value_;
    }

    public String getId_() {
        return id_;
    }

    @Override
    public String toString() {
        return "["+markType_+","+absent_+","+value_+","+studentId_+","+isPredicted_+"]";
    }
}
