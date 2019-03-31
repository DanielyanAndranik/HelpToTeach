package ModelsV2;

import java.io.Serializable;

public class KeyStudent implements Serializable {

    private String fn_;
    private String ln_;
    private String mn_;
    private String gn_;

    public KeyStudent(String fn,String ln,String mn,String gn){
        fn_ = fn;
        ln_ = ln;
        gn_ = gn;
        mn_ = mn;
    }



    @Override
    public boolean equals(Object other) {
        if (other == null) return false;
        if (other == this) return true;
        if (!(other instanceof KeyStudent))return false;
        KeyStudent otherMyClass = (KeyStudent) other;

        return this.hashCode() == otherMyClass.hashCode();

    }


    @Override
    public int hashCode() {
        return fn_.hashCode() + ln_.hashCode() + gn_.hashCode() + mn_.hashCode();
    }

    public String getFn_() {
        return fn_;
    }

    public String getGn_() {
        return gn_;
    }

    public String getLn_() {
        return ln_;
    }

    public String getMn_() {
        return mn_;
    }

    public void setFn_(String fn_) {
        this.fn_ = fn_;
    }

    public void setGn_(String gn_) {
        this.gn_ = gn_;
    }

    public void setMn_(String mn_) {
        this.mn_ = mn_;
    }

    public void setLn_(String ln_) {
        this.ln_ = ln_;
    }

    @Override
    public String toString() {
        return fn_+","+ln_+","+mn_+","+gn_;
    }
}
