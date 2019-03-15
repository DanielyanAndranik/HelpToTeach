import org.apache.spark.api.java.function.Function;

public class HeaderFilter implements Function<String,Boolean> {
    @Override
    public Boolean call(String s) throws Exception {
        return !(s.contains("id") || s.contains("Fname") || s.contains("Lname"));
    }
}
