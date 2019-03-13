import ModelsV2.Course;
import ModelsV2.KeyStudent;
import ModelsV2.Mark;
import ModelsV2.Student;
import com.cedarsoftware.util.io.JsonWriter;
import com.couchbase.client.java.document.JsonDocument;
import com.couchbase.client.java.document.json.JsonArray;
import com.couchbase.client.java.document.json.JsonObject;
import com.couchbase.spark.japi.CouchbaseDocumentRDD;
import com.couchbase.spark.japi.CouchbaseSparkContext;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.google.gson.Gson;
import org.apache.avro.generic.GenericData;
import org.apache.spark.SparkConf;
import org.apache.spark.api.java.JavaPairRDD;
import org.apache.spark.api.java.JavaRDD;
import org.apache.spark.api.java.JavaSparkContext;
import org.apache.spark.api.java.function.Function;
import org.apache.spark.api.java.function.Function2;
import org.apache.spark.api.java.function.PairFunction;
import scala.Tuple2;

import java.security.Key;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import java.util.UUID;
import java.util.logging.Level;
import java.util.logging.Logger;
import ModelsV2.Group;

public class MainV2 {

    private static List<String> coursesList = new ArrayList<>();
    private static List<String> groupsList = new ArrayList<>();

    public static void main(String args[]){
        Logger.getLogger("org").setLevel(Level.ALL);

        SparkConf conf = new SparkConf()
                .setAppName("HelpToTeach")
                .setMaster("local[*]")
                .set("com.couchbase.nodes", "127.0.0.1:8091")
                .set("com.couchbase.bucket.HelpToTeachBucket", "password");

        JavaSparkContext sparkContext = new JavaSparkContext(conf);
        CouchbaseSparkContext couchbaseContext = CouchbaseSparkContext.couchbaseContext(sparkContext);


        coursesList.add(new Course(UUID.randomUUID().toString(),".Net").toString());
        coursesList.add(new Course(UUID.randomUUID().toString(),"Machine Learning").toString());
        coursesList.add(new Course(UUID.randomUUID().toString(),"Calculus").toString());
        coursesList.add(new Course(UUID.randomUUID().toString(),"Image Processing").toString());

        coursesList.add(new Course(UUID.randomUUID().toString(),"SQL").toString());
        coursesList.add(new Course(UUID.randomUUID().toString(),"Java").toString());

        groupsList.add(new Group(UUID.randomUUID().toString(),"119-1").toString());
        groupsList.add(new Group(UUID.randomUUID().toString(),"119-2").toString());
        groupsList.add(new Group(UUID.randomUUID().toString(),"119-3").toString());
        groupsList.add(new Group(UUID.randomUUID().toString(),"219-1").toString());
        groupsList.add(new Group(UUID.randomUUID().toString(),"219-2").toString());

        //-------big data processing------------------------------------------------------------------------


        JavaRDD<Group> groupsRDD = sparkContext.parallelize(groupsList)
                .map(new Function<String, Group>() {
                    @Override
                    public Group call(String s) throws Exception {
                        String[] temp = s.split(",");
                        return new Group(temp[0],temp[1]);
                    }
                });
        JavaRDD<Course> coursesRDD = sparkContext.parallelize(coursesList)
                .map(new Function<String, Course>() {
                    @Override
                    public Course call(String s) throws Exception {
                        String[] temp = s.split(",");
                        return new Course(temp[0],temp[1]);
                    }
                });

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

        //making pair rdds

        JavaPairRDD<KeyStudent, Mark> dotNet2011PairRDD = dotNet2011.mapToPair(new PairCreater("Net"));
        JavaPairRDD<KeyStudent, Mark> calculus2011PairRDD = calculus2011.mapToPair(new PairCreater("Calculus"));
        JavaPairRDD<KeyStudent,Mark> imageProcessing2011PairRDD = imageProcessing2011.mapToPair(new PairCreater("Image"));
        JavaPairRDD<KeyStudent,Mark> machineLearning2011PairRDD = machineLearning2011.mapToPair(new PairCreater("Machine"));

        JavaPairRDD<KeyStudent,Iterable<Mark>> allDataFor2011 = dotNet2011PairRDD
                .union(calculus2011PairRDD)
                .union(imageProcessing2011PairRDD)
                .union(machineLearning2011PairRDD)
                .groupByKey();

        JavaRDD<Student> resultFor2011 = allDataFor2011.map(new Function<Tuple2<KeyStudent, Iterable<Mark>>, Student>() {
            @Override
            public Student call(Tuple2<KeyStudent, Iterable<Mark>> keyStudentIterableTuple2) throws Exception {
                Student student = new Student();
                student.setId_(UUID.randomUUID().toString());
                student.setFirstName_(keyStudentIterableTuple2._1.getFn_());
                student.setLastName_(keyStudentIterableTuple2._1.getLn_());
                student.setGroupId_(keyStudentIterableTuple2._1.getGn_());
                List<Mark> marks = new ArrayList<>();

                for (Mark m:
                     keyStudentIterableTuple2._2) {
                    marks.add(m);
                }

                student.setMarks_(marks);
                return student;
            }
        });

        JavaRDD<JsonDocument> couchbaseResult = resultFor2011.map(new Function<Student, JsonDocument>() {
            @Override
            public JsonDocument call(Student student) throws Exception {

                ObjectMapper maper = new ObjectMapper();


                String marksJson = maper.writeValueAsString(student.getMarks_());


                JsonObject data = JsonObject.create()
                        .put("firstName",student.getFirstName_())
                        .put("lastName",student.getLastName_())
                        .put("id",student.getId_())
                        .put("type","student")
                        .put("marks",marksJson)
                        .put("groupId",student.getGroupId_());

                return JsonDocument.create("student::"+student.getId_(),data);
            }
        });

        CouchbaseDocumentRDD<JsonDocument> result1 = CouchbaseDocumentRDD.couchbaseDocumentRDD(couchbaseResult);
        result1.saveToCouchbase();

    }

    static class PairCreater implements PairFunction<String,KeyStudent,Mark>{

        private String courseName;

        public PairCreater(String courseName){
            this.courseName = courseName;
        }

        @Override
        public Tuple2<KeyStudent, Mark> call(String s) throws Exception {
            String temp[] = s.split(",");
            String firstName = temp[1];
            String lastName = temp[2];
            String group = temp[3];

            String courseId = "???";

            for (String lines: coursesList) {
                if(lines.split(",")[1].contains(courseName)){
                    courseId = lines.split(",")[0];
                    break;
                }
            }

            for (String lines:
                 groupsList) {
                if(lines.contains(group)){
                    group = lines.split(",")[0];
                    break;
                }
            }

            return new Tuple2<KeyStudent,Mark>(new KeyStudent(firstName,lastName,group),
                    new Mark(courseId,temp[4],temp[5],temp[6],temp[7]));

        }
    }


}
