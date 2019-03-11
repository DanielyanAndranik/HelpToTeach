import ModelsV2.Course;
import ModelsV2.Mark;
import com.couchbase.client.java.document.JsonDocument;
import com.couchbase.client.java.document.json.JsonObject;
import com.couchbase.spark.japi.CouchbaseDocumentRDD;
import com.couchbase.spark.japi.CouchbaseSparkContext;
import org.apache.avro.generic.GenericData;
import org.apache.spark.SparkConf;
import org.apache.spark.api.java.JavaPairRDD;
import org.apache.spark.api.java.JavaRDD;
import org.apache.spark.api.java.JavaSparkContext;
import org.apache.spark.api.java.function.Function;
import org.apache.spark.api.java.function.PairFunction;
import scala.Tuple2;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.logging.Level;
import java.util.logging.Logger;

public class MainV2 {

    public static void main(String args[]){
        Logger.getLogger("org").setLevel(Level.ALL);

        SparkConf conf = new SparkConf()
                .setAppName("HelpToTeach")
                .setMaster("local[*]")
                .set("com.couchbase.nodes", "127.0.0.1:8091")
                .set("com.couchbase.bucket.HelpToTeachBucket", "password");

        JavaSparkContext sparkContext = new JavaSparkContext(conf);
        CouchbaseSparkContext couchbaseContext = CouchbaseSparkContext.couchbaseContext(sparkContext);


        List<String> coursesList = new ArrayList<>();
        coursesList.add(new Course(UUID.randomUUID().toString(),".Net").toString());
        coursesList.add(new Course(UUID.randomUUID().toString(),"Machine Learning").toString());
        coursesList.add(new Course(UUID.randomUUID().toString(),"Calculus").toString());
        coursesList.add(new Course(UUID.randomUUID().toString(),"Image Processing").toString());

        coursesList.add(new Course(UUID.randomUUID().toString(),"SQL").toString());
        coursesList.add(new Course(UUID.randomUUID().toString(),"Java").toString());

        //-------big data processing------------------------------------------------------------------------


        JavaRDD<String> dotNet2011 = sparkContext.textFile(InputPaths2011.DotNet)
                .filter(new HeaderFilter());
        JavaRDD<String> calculus2011 = sparkContext.textFile(InputPaths2011.Calculus)
                .filter(new HeaderFilter());
        JavaRDD<String> imageProcessing2011 = sparkContext.textFile(InputPaths2011.ImageProcessing)
                .filter(new HeaderFilter());
        JavaRDD<String> machineLearning2011 = sparkContext.textFile(InputPaths2011.MachineLearning)
                .filter(new HeaderFilter());

        JavaRDD<String> dotNet2012 = sparkContext.textFile(InputPaths2012.DotNet)
                .filter(new HeaderFilter());
        JavaRDD<String> calculus2012 = sparkContext.textFile(InputPaths2012.Calculus)
                .filter(new HeaderFilter());
        JavaRDD<String> imageProcessing2012 = sparkContext.textFile(InputPaths2012.ImageProcessing)
                .filter(new HeaderFilter());
        JavaRDD<String> machineLearning2012 = sparkContext.textFile(InputPaths2012.MachineLearning)
                .filter(new HeaderFilter());


        JavaRDD<String> java2012 = sparkContext.textFile(InputPaths2012.Java)
                .filter(new HeaderFilter());
        JavaRDD<String> sql2012 = sparkContext.textFile(InputPaths2012.SQL)
                .filter(new HeaderFilter());


        JavaPairRDD<String, Mark> dnotNet2011PairRDD = dotNet2011.mapToPair(new PairFunction<String, String, Mark>() {
            @Override
            public Tuple2<String, Mark> call(String s) throws Exception {
                String temp[] = s.split(",");
                String firstName = temp[1];
                String lastName = temp[2];
                String group = temp[3];

                String courseId = "???";

                for (String lines: coursesList) {
                    if(lines.split(",")[1].contains("Net")){
                        courseId = lines.split(",")[0];
                        break;
                    }
                }

                return new Tuple2<String,Mark>(firstName+","+lastName+","+group,
                        new Mark(courseId,temp[4],temp[5],temp[6],temp[7]));

            }
        });

        dnotNet2011PairRDD.saveAsTextFile("output/text.txt");

    }



}
