import Models.Teacher;
import com.couchbase.client.java.document.JsonDocument;
import com.couchbase.client.java.document.json.JsonObject;
import com.couchbase.spark.japi.CouchbaseDocumentRDD;
import com.couchbase.spark.japi.CouchbaseSparkContext;
import org.apache.spark.SparkConf;
import org.apache.spark.api.java.JavaRDD;
import org.apache.spark.api.java.JavaSparkContext;
import org.apache.spark.api.java.function.Function;
import org.apache.spark.sql.SparkSession;


import java.util.UUID;


public class Main {

    public static void main(String args[]){


        SparkConf conf = new SparkConf()
                .setAppName("HelpToTeach")
                .setMaster("local[*]")
                .set("com.couchbase.nodes", "127.0.0.1:8091")
                .set("com.couchbase.bucket.HelpToTeachBucket", "password");

        JavaSparkContext sparkContext = new JavaSparkContext(conf);
        CouchbaseSparkContext couchbaseContext = CouchbaseSparkContext.couchbaseContext(sparkContext);



        JavaRDD<String> teachers = sparkContext.textFile("input/2010/Teachers-2010.txt");



        JavaRDD<String> teachers_id = teachers.map(new Function<String, String>() {
            @Override
            public String call(String s) throws Exception {
                String temp[] = s.split(",");
                return UUID.randomUUID().toString()+","+temp[1];
            }
        });

        JavaRDD<JsonDocument> teacher_documents = teachers_id.map(new Function<String, JsonDocument>() {
            @Override
            public JsonDocument call(String s) throws Exception {
                Teacher teacher = new Teacher();
                String temp[] = s.split(",");
                teacher.setId(temp[0]);
                teacher.setFirstName(temp[1].split(" ")[0]);
                teacher.setLastName(temp[1].split(" ")[1]);

                JsonObject data = JsonObject.create()
                        .put("firstName",teacher.getFirstName())
                        .put("lastName",teacher.getLastName());


                return JsonDocument.create("teacher::"+teacher.getId(),data);

            }
        });

        //teacher_documents.saveAsTextFile("output.result.txt");

        CouchbaseDocumentRDD<JsonDocument> result = CouchbaseDocumentRDD.couchbaseDocumentRDD(teacher_documents);
        result.saveToCouchbase();

    }

}
