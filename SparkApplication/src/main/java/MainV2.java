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
import com.google.gson.GsonBuilder;
import com.google.gson.JsonElement;
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

        JavaRDD<String> dotNet2011 = sparkContext.textFile(InputPaths2011.DotNet);
        JavaRDD<String> calculus2011 = sparkContext.textFile(InputPaths2011.Calculus);
        JavaRDD<String> imageProcessing2011 = sparkContext.textFile(InputPaths2011.ImageProcessing);
        JavaRDD<String> machineLearning2011 = sparkContext.textFile(InputPaths2011.MachineLearning);

        JavaRDD<String> dotNet2012 = sparkContext.textFile(InputPaths2012.DotNet);
        JavaRDD<String> calculus2012 = sparkContext.textFile(InputPaths2012.Calculus);
        JavaRDD<String> imageProcessing2012 = sparkContext.textFile(InputPaths2012.ImageProcessing);
        JavaRDD<String> machineLearning2012 = sparkContext.textFile(InputPaths2012.MachineLearning);


        JavaRDD<String> java2012 = sparkContext.textFile(InputPaths2012.Java);
        JavaRDD<String> sql2012 = sparkContext.textFile(InputPaths2012.SQL);

        //making pair rdds

        JavaPairRDD<KeyStudent,Mark> dotNet2011PairRDD = dotNet2011.mapToPair(new PairCreater("Net"));
        JavaPairRDD<KeyStudent, Mark> calculus2011PairRDD = calculus2011.mapToPair(new PairCreater("Calculus"));
        JavaPairRDD<KeyStudent,Mark> imageProcessing2011PairRDD = imageProcessing2011.mapToPair(new PairCreater("Image"));
        JavaPairRDD<KeyStudent,Mark> machineLearning2011PairRDD = machineLearning2011.mapToPair(new PairCreater("Machine"));

        JavaPairRDD<KeyStudent, Mark> dotNet2012PairRDD = dotNet2012.mapToPair(new PairCreater("Net"));
        JavaPairRDD<KeyStudent, Mark> calculus2012PairRDD = calculus2012.mapToPair(new PairCreater("Calculus"));
        JavaPairRDD<KeyStudent,Mark> imageProcessing2012PairRDD = imageProcessing2012.mapToPair(new PairCreater("Image"));
        JavaPairRDD<KeyStudent,Mark> machineLearning2012PairRDD = machineLearning2012.mapToPair(new PairCreater("Machine"));

        JavaPairRDD<KeyStudent, Mark> java2012PairRDD = java2012.mapToPair(new PairCreater("Java"));
        JavaPairRDD<KeyStudent, Mark> sql2012PairRDD = sql2012.mapToPair(new PairCreater("SQL"));


        JavaPairRDD<KeyStudent,Iterable<Mark>> allDataFor2011 = dotNet2011PairRDD
                .union(calculus2011PairRDD)
                .union(imageProcessing2011PairRDD)
                .union(machineLearning2011PairRDD)
                .union(dotNet2012PairRDD)
                .union(calculus2012PairRDD)
                .union(imageProcessing2012PairRDD)
                .union(machineLearning2012PairRDD)
                .union(java2012PairRDD)
                .union(sql2012PairRDD)
                .groupByKey();

        JavaRDD<Student> resultFor2011and2012 = allDataFor2011.map(new Function<Tuple2<KeyStudent, Iterable<Mark>>, Student>() {
            @Override
            public Student call(Tuple2<KeyStudent, Iterable<Mark>> keyStudentIterableTuple2) throws Exception {
                Student student = new Student();
                student.setId_(UUID.randomUUID().toString());
                student.setFirstName_(keyStudentIterableTuple2._1.getFn_());
                student.setLastName_(keyStudentIterableTuple2._1.getLn_());
                student.setMiddleName_(keyStudentIterableTuple2._1.getMn_());
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

        JavaRDD<JsonDocument> couchbaseStudents2011Result = resultFor2011and2012.map(new Function<Student, JsonDocument>() {
            @Override
            public JsonDocument call(Student student) throws Exception {


                JsonArray jsonMarks = JsonArray.create();
                for (Mark mark:
                     student.getMarks_()) {
                    JsonObject tempMark = JsonObject.create()
                            .put("coursId",mark.getCourseId())
                            .put("first",mark.getFirst())
                            .put("second",mark.getSecond())
                            .put("finalMark",mark.getFinal())
                            .put("presence",mark.getN());
                    jsonMarks.add(tempMark);
                }


                JsonObject data = JsonObject.create()
                        .put("firstName",student.getFirstName_())
                        .put("lastName",student.getLastName_())
                        .put("id",student.getId_())
                        .put("middleName",student.getMiddleName_())
                        .put("type","student")
                        .put("marks",jsonMarks)
                        .put("groupId",student.getGroupId_());

                return JsonDocument.create("student::"+student.getId_(),data);
            }
        });

        CouchbaseDocumentRDD<JsonDocument> result1 = CouchbaseDocumentRDD.couchbaseDocumentRDD(couchbaseStudents2011Result);
        result1.saveToCouchbase();

        JavaRDD<JsonDocument> couchbaseGroupsResult = groupsRDD.map(new Function<Group, JsonDocument>() {
            @Override
            public JsonDocument call(Group group) throws Exception {
                JsonObject data = JsonObject.create()
                        .put("groupId",group.getId_())
                        .put("groupName",group.getName_())
                        .put("type","group");

                return JsonDocument.create("group::"+group.getId_(),data);
            }
        });

        CouchbaseDocumentRDD<JsonDocument> result2 = CouchbaseDocumentRDD.couchbaseDocumentRDD(couchbaseGroupsResult);
        result2.saveToCouchbase();

        JavaRDD<JsonDocument> couchbaseCoursesResult = coursesRDD.map(new Function<Course, JsonDocument>() {
            @Override
            public JsonDocument call(Course course) throws Exception {
                JsonObject data = JsonObject.create()
                        .put("courseId",course.getId_())
                        .put("courseName",course.getName_())
                        .put("type","course");

                return JsonDocument.create("course::"+course.getId_(),data);
            }
        });

        CouchbaseDocumentRDD<JsonDocument> result3 = CouchbaseDocumentRDD.couchbaseDocumentRDD(couchbaseCoursesResult);
        result3.saveToCouchbase();


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
            String middleName = temp[3];
            String group = temp[4];

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

            return new Tuple2<KeyStudent,Mark>(new KeyStudent(firstName,lastName,middleName,group),
                    new Mark(courseId,temp[5],temp[6],temp[7],temp[8]));

        }
    }


}
