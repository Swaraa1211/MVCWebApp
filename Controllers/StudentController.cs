using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MVCWebApplication.Models;

namespace MVCWebApplication.Controllers
{
    public class StudentController : Controller
    {
        IConfiguration configuration;
        SqlConnection SqlConnection;
        public StudentController(IConfiguration configuration)
        {
            this.configuration = configuration;
            SqlConnection = new SqlConnection(configuration.GetConnectionString("TryDB"));
        }

        StudentModel GetStudent(int id)
        {
            //List<StudentModel> StudentList = new List<StudentModel>();
            SqlConnection.Open();
            SqlCommand cmd = new SqlCommand("GET_STUDENT", SqlConnection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@studentId", id);

            SqlDataReader reader = cmd.ExecuteReader();
            StudentModel student = new StudentModel();

            while (reader.Read())
            {
                student.StudentId = (int)reader["StuId"];
                student.StudentName = (string)reader["StuName"];
                student.CourseName = (string)reader["CourseName"];
                student.RegisterNumber = (int)reader["RegisterNum"];
                student.Marks = (int)reader["Marks"];
                //StudentList.Add(student);
            }

            reader.Close();
            SqlConnection.Close();

            return student;
        }

        public List<StudentModel> GetStudentsList()
        {
            List<StudentModel> StudentList = new List<StudentModel>();
            SqlConnection.Open();
            SqlCommand cmd = new SqlCommand("FETCH_STUDENTS", SqlConnection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                StudentModel student = new StudentModel();
                student.StudentId = (int)reader["StuId"];
                student.StudentName = (string)reader["StuName"];
                student.CourseName = (string)reader["CourseName"];
                student.RegisterNumber = (int)reader["RegisterNum"];
                student.Marks = (int)reader["Marks"];
                StudentList.Add(student);
            }
            reader.Close();
            SqlConnection.Close();


            return StudentList;

        }
        // GET: StudentController
        public ActionResult Index()
        {
            //SqlConnection conn = new SqlConnection(connectionString);
            return View(GetStudentsList());
        }

        // GET: StudentController/Details/5
        public ActionResult Details(int id)
        {
            return View(GetStudent(id));
        }

        // GET: StudentController/Create
        public ActionResult Create()
        {
            return View();
        }
        void InsertStudents(StudentModel student)
        {
            List<StudentModel> StudentList = new List<StudentModel>();
            SqlConnection.Open();
            SqlCommand cmd = new SqlCommand("ADD_STUDENT", SqlConnection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@studentName", student.StudentName);
            cmd.Parameters.AddWithValue("@course", student.CourseName);
            cmd.Parameters.AddWithValue("@regNum", student.RegisterNumber);
            cmd.Parameters.AddWithValue("@marks", student.Marks);


            cmd.ExecuteNonQuery();
            SqlConnection.Close();
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentModel student)
        {
            try
            {
                Console.WriteLine(student.StudentName);
                InsertStudents(student);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        
        // GET: StudentController/Edit/5
        public ActionResult Edit(int id)
        {


            return View(GetStudent(id));
        }

        void UpdateStudent(int id, StudentModel student)
        {
            //List<StudentModel> StudentList = new List<StudentModel>();
            SqlConnection.Open();
            SqlCommand cmd = new SqlCommand("EDIT_STUDENT", SqlConnection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@studentName", student.StudentName);
            cmd.Parameters.AddWithValue("@course", student.CourseName);
            cmd.Parameters.AddWithValue("@regNum", student.RegisterNumber);
            cmd.Parameters.AddWithValue("@marks", student.Marks);
            cmd.Parameters.AddWithValue("@studentId", id);


            cmd.ExecuteNonQuery();
            SqlConnection.Close();
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, StudentModel student)
        {
            try
            {
                UpdateStudent(id,student);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //void DeleteStudent(int id)
        //{
        //    SqlConnection.Open();
        //    SqlCommand cmd = new SqlCommand("DELETE_STUDENT", SqlConnection);
        //    cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //    cmd.Parameters.AddWithValue("@studentId", id);

        //    cmd.ExecuteNonQuery();
        //    SqlConnection.Close();
        //    //return student;
        //}
        // GET: StudentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(GetStudent(id));
        }
        // POST: StudentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, StudentModel student)
        {
            try
            {
                SqlConnection.Open();
                //SqlCommand cmd = new SqlCommand("DELETE_STUDENT", SqlConnection);
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlCommand cmd = new SqlCommand("DELETE FROM TStudents1 WHERE StuId = @studentId", SqlConnection);

                cmd.Parameters.AddWithValue("@studentId", id);

                cmd.ExecuteNonQuery();
                SqlConnection.Close();

                //DeleteStudent(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
