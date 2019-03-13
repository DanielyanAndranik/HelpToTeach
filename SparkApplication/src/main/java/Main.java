import Models.Course;
import Models.Group;
import Models.Student;
import Models.Teacher;
import com.couchbase.client.java.document.JsonDocument;
import com.couchbase.client.java.document.json.JsonObject;
import com.couchbase.spark.japi.CouchbaseDocumentRDD;
import com.couchbase.spark.japi.CouchbaseSparkContext;
import org.apache.spark.SparkConf;
import org.apache.spark.api.java.JavaPairRDD;
import org.apache.spark.api.java.JavaRDD;
import org.apache.spark.api.java.JavaSparkContext;
import org.apache.spark.api.java.function.FlatMapFunction;
import org.apache.spark.api.java.function.Function;
import org.apache.spark.api.java.function.PairFunction;
import org.apache.spark.sql.SparkSession;
import org.codehaus.janino.Java;
import scala.Array;
import scala.Tuple2;


import java.util.Date;
import java.util.Iterator;
import java.util.List;
import java.util.UUID;
import java.util.logging.Level;
import java.util.logging.Logger;


public class Main {

    private static final String COURSES_PATH = "input/2010/Courses-2010.txt";
    private static final String GROUPS_PATH = "input/2010/Groups-2010.txt";
    private static final String STUDENTS_PATH = "input/2010/Students-2010.txt";
    private static final String TEACHERS_PATH = "input/2010/Teachers-2010.txt";



    public static void main(String args[]){

        Logger.getLogger("org").setLevel(Level.ALL);

        SparkConf conf = new SparkConf()
                .setAppName("HelpToTeach")
                .setMaster("local[*]")
                .set("com.couchbase.nodes", "127.0.0.1:8091")
                .set("com.couchbase.bucket.HelpToTeachBucket", "password");

        JavaSparkContext sparkContext = new JavaSparkContext(conf);
        CouchbaseSparkContext couchbaseContext = CouchbaseSparkContext.couchbaseContext(sparkContext);

        JavaRDD<String> coursesRawData = sparkContext.textFile(COURSES_PATH);
        JavaRDD<String> groupsRawData = sparkContext.textFile(GROUPS_PATH);
        JavaRDD<String> studentsRawData = sparkContext.textFile(STUDENTS_PATH)
                .filter(new Function<String, Boolean>() {
                    @Override
                    public Boolean call(String s) throws Exception {
                        return !(s.contains(".....") && s.contains("id") && s.contains("group"));
                    }
                });
        JavaRDD<String> teachersRawData = sparkContext.textFile(TEACHERS_PATH);

        JavaRDD<Course> courses = coursesRawData.map(new Function<String, Course>() {
            @Override
            public Course call(String s) throws Exception {

                String temp[] = s.split(",");
                Course course = new Course();

                course.setId(UUID.randomUUID().toString());
                course.setName(temp[1]);
                course.setType("course");

                return course;

            }
        });

        //courses.saveAsTextFile("output/courses.txt");

        JavaRDD<Teacher> teachers = teachersRawData.map(new Function<String, Teacher>() {
            @Override
            public Teacher call(String s) throws Exception {
                String temp[] = s.split(",");
                Teacher teacher = new Teacher();

                teacher.setId(UUID.randomUUID().toString());
                teacher.setType("teacher");
                teacher.setFirstName(temp[1].split(" ")[0]);
                teacher.setLastName(temp[1].split(" ")[1]);
                return teacher;
            }
        });

        //teachers.saveAsTextFile("output/teachers.txt");


        //-------------------------------------Logic for bindig groups with students------------------

        JavaRDD<Group> groups = groupsRawData.map(new Function<String, Group>() {
            @Override
            public Group call(String s) throws Exception {
                String temp[] = s.split(",");
                Group group = new Group();

                group.setName(temp[1]);
                group.setType("group");
                group.setId(UUID.randomUUID().toString());

                return group;
            }
        });




        JavaPairRDD<String,String> groupNameIdPair = groups.mapToPair(new PairFunction<Group, String, String>() {
            @Override
            public Tuple2<String, String> call(Group group) throws Exception {
                return new Tuple2<String,String>(group.getName(),group.getId());
            }
        });

        JavaPairRDD<String,String> groupStudentInfoPair = studentsRawData.mapToPair(new PairFunction<String, String, String>() {
            @Override
            public Tuple2<String, String> call(String s) throws Exception {
                String temp[] = s.split(",");

                Student student = new Student();
                student.setId(UUID.randomUUID().toString());
                student.setType("student");
                student.setFirstName(temp[1]);
                student.setLastName(temp[2]);
                student.setGroupId("???");

                return new Tuple2<String, String>(temp[3],student.toString());
            }
        });


        JavaPairRDD<String,Tuple2<String,String>> result = groupNameIdPair.join(groupStudentInfoPair);


        JavaRDD<Student> studentResult = result.map(new Function<Tuple2<String, Tuple2<String, String>>, Student>() {
            @Override
            public Student call(Tuple2<String, Tuple2<String, String>> stringTuple2Tuple2) throws Exception {
                Student student = new Student();
                student.setType("student");
                student.setGroupId(stringTuple2Tuple2._2._1);
                student.setFirstName(stringTuple2Tuple2._2._2.split(",")[2]);
                student.setLastName(stringTuple2Tuple2._2._2.split(",")[3]);
                student.setId(stringTuple2Tuple2._2._2.split(",")[0]);

                return student;

            }
        });

        JavaPairRDD<String, Iterable<Tuple2<String, String>>> iHope = result.groupByKey();

        groups = iHope.map(new Function<Tuple2<String, Iterable<Tuple2<String, String>>>, Group>() {
            @Override
            public Group call(Tuple2<String, Iterable<Tuple2<String, String>>> stringIterableTuple2) throws Exception {
                Group group = new Group();
                group.setType("group");
                group.setName(stringIterableTuple2._1);
                group.setId(stringIterableTuple2._2.iterator().next()._1);
                return group;
            }
        });



        JavaRDD<JsonDocument> groups_document = groups.map(new Function<Group, JsonDocument>() {
            @Override
            public JsonDocument call(Group group) throws Exception {
                JsonObject data = JsonObject.create()
                        .put("name",group.getName())
                        .put("id",group.getId())
                        .put("type",group.getType());
                return JsonDocument.create("group::"+group.getId(),data);
            }
        });
        JavaRDD<JsonDocument> students_document = studentResult.map(new Function<Student, JsonDocument>() {
            @Override
            public JsonDocument call(Student student) throws Exception {
                JsonObject data = JsonObject.create()
                        .put("firstName",student.getFirstName())
                        .put("lastName",student.getLastName())
                        .put("id",student.getId())
                        .put("groupId",student.getGroupId())
                        .put("type",student.getType());

                return JsonDocument.create("student::"+student.getId(),data);
            }
        });

        JavaRDD<JsonDocument> teacher_documents = teachers.map(new Function<Teacher, JsonDocument>() {
            @Override
            public JsonDocument call(Teacher teacher) throws Exception {
                JsonObject data = JsonObject.create()
                        .put("firstName",teacher.getFirstName())
                        .put("lastName",teacher.getLastName())
                        .put("id",teacher.getId())
                        .put("type","teacher");

                return JsonDocument.create("teacher::"+teacher.getId(),data);
            }
        });

        JavaRDD<JsonDocument> courses_documents = courses.map(new Function<Course, JsonDocument>() {
            @Override
            public JsonDocument call(Course course) throws Exception {
                JsonObject data = JsonObject.create()
                        .put("name",course.getName())
                        .put("id",course.getId())
                        .put("type",course.getType());

                return JsonDocument.create("course::"+course.getId(),data);
            }
        });


        CouchbaseDocumentRDD<JsonDocument> result1 = CouchbaseDocumentRDD.couchbaseDocumentRDD(teacher_documents);
        result1.saveToCouchbase();

        CouchbaseDocumentRDD<JsonDocument> result2 = CouchbaseDocumentRDD.couchbaseDocumentRDD(courses_documents);
        result2.saveToCouchbase();

        CouchbaseDocumentRDD<JsonDocument> result3 = CouchbaseDocumentRDD.couchbaseDocumentRDD(students_document);
        result3.saveToCouchbase();

        CouchbaseDocumentRDD<JsonDocument> result4 = CouchbaseDocumentRDD.couchbaseDocumentRDD(groups_document);
        result4.saveToCouchbase();



        //-------------------------------------Logic for bindig groups with students end------------------

        //groups.saveAsTextFile("output/groups.txt");

        /*
        JavaRDD<Student> students = studentsRawData.map(new Function<String, Student>() {
            @Override
            public Student call(String s) throws Exception {
                String temp[] = s.split(",");
                List<Group> groupsList = groups.collect();

                Student student = new Student();
                student.setId(UUID.randomUUID().toString());
                student.setType("student");
                student.setFirstName(temp[1]);
                student.setLastName(temp[2]);

                for (Group g:groupsList) {
                    if(g.getName() == temp[3]){
                        student.setGroupId(g.getId());
                    }
                    else{
                        student.setGroupId("invalid");
                    }
                }
                return student;
            }
        }).filter(new Function<Student, Boolean>() {
            @Override
            public Boolean call(Student student) throws Exception {
                return !(student.getGroupId() == "invalid");
            }
        });

        students.saveAsTextFile("output/students.txt");
        */

        /*
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
                        .put("lastName",teacher.getLastName())
                        .put("id",teacher.getId())
                        .put("type","teacher");

                return JsonDocument.create("teacher::"+teacher.getId(),data);

            }
        });

        //teacher_documents.saveAsTextFile("output.result.txt");

        CouchbaseDocumentRDD<JsonDocument> result = CouchbaseDocumentRDD.couchbaseDocumentRDD(teacher_documents);
        result.saveToCouchbase();
        */
    }



}
