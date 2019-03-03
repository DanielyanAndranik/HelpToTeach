import org.apache.commons.lang.StringUtils;
import org.apache.spark.SparkConf;
import org.apache.spark.api.java.JavaRDD;
import org.apache.spark.api.java.JavaSparkContext;


public class Main {

    public static void main(String[] args){

        SparkConf conf = new SparkConf()
                .setAppName("HelpToTeach")
                .setMaster("local[2]");
        
        JavaSparkContext context = new JavaSparkContext(conf);

        JavaRDD<String> airports = context.textFile("input/airports.txt");

        airports.saveAsTextFile("output/airports_by_latitude.text");

    }

}
