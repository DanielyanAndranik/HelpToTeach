import com.couchbase.spark.japi.CouchbaseSparkContext;
import org.apache.commons.lang.StringUtils;
import org.apache.spark.SparkConf;
import org.apache.spark.api.java.JavaRDD;
import org.apache.spark.api.java.JavaSparkContext;


public class Main {

    public static void main(String[] args){

        SparkConf config = new SparkConf()
                .setAppName("HelpToTeach")
                .setMaster("local[*]")
                .set("com.couchbase.bucket.HelpToTeachMainBucket", "password");

        JavaSparkContext context = new JavaSparkContext(config);
        CouchbaseSparkContext csc = CouchbaseSparkContext.couchbaseContext(context);

    }

}
