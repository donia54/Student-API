using Microsoft.Data.SqlClient;
using System.Data;

namespace StudentApiDataAccesseLayer
{
    public class StudentData
    {

        public static List<StudentDTO> GetAllStudents()
        {
            var StudentsList = new List<StudentDTO>(); //to hold the retrieved student data

            using(SqlConnection connection=new SqlConnection(DataAccessSetting.ConnectionString))
            {
                //all the stored procedure named "SP_GetAllStudents"
                using (SqlCommand command=new SqlCommand("SP_GetAllStudents", connection)) 
                {
                    command.CommandType=CommandType.StoredProcedure; // Specify that the command is a stored procedure
                    connection.Open();

                    using(SqlDataReader reader=command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StudentsList.Add(new StudentDTO
                             (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))
                             ));
                        }
                    }
                }
            }
            return StudentsList;
        }

        public static List<StudentDTO> GetPassedStudents()
        {
            var StudentsList = new List<StudentDTO>();

            using(SqlConnection connection=new SqlConnection( DataAccessSetting.ConnectionString))
            {
                using (SqlCommand command=new SqlCommand("SP_GetPassedStudents",connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using( SqlDataReader reader=command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            StudentsList.Add(new StudentDTO
                             (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))
                             ));
                        }
                    }

                }
            }





            return StudentsList;

        }

        public static double GetAvgGrade()
        {
            double avgGrade = 0;

            using(SqlConnection connection=new SqlConnection(DataAccessSetting.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAverageGrade",connection))
                {
                    command.CommandType= CommandType.StoredProcedure;
                    connection.Open();
                    object result=command.ExecuteScalar();
                    if(result!=DBNull.Value)
                        avgGrade=Convert.ToDouble(result);              
                }

            }

            return avgGrade;
        }

        public static StudentDTO GetStudentByID(int id)
        {
            using (SqlConnection connection = new SqlConnection(StudentApiDataAccesseLayer.DataAccessSetting.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetStudentByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", id);
                    connection.Open();

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new StudentDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))
                            );
                        }
                        else
                            return null;
                    }

                }
            }

        }

        public static int AddStudent(StudentDTO studentDTO)
        {
            using (var connection = new SqlConnection(DataAccessSetting.ConnectionString))
            {
                using (var command = new SqlCommand("SP_AddStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    command.Parameters.AddWithValue("@Name", studentDTO.Name);
                    command.Parameters.AddWithValue("@Age", studentDTO.Age);
                    command.Parameters.AddWithValue("@Grade", studentDTO.Grade);
                    var outputIDParam = new SqlParameter("@NewStudentId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputIDParam);
                    command.ExecuteNonQuery();
                    return (int)outputIDParam.Value;
                }
            }
        }

        public static bool UpdateStudent(StudentDTO StudentDTO)
        {
            using (var connection = new SqlConnection(StudentApiDataAccesseLayer.DataAccessSetting.ConnectionString))
            using (var command = new SqlCommand("SP_UpdateStudent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID", StudentDTO.Id);
                command.Parameters.AddWithValue("@Name", StudentDTO.Name);
                command.Parameters.AddWithValue("@Age", StudentDTO.Age);
                command.Parameters.AddWithValue("@Grade", StudentDTO.Grade);

                connection.Open();
                command.ExecuteNonQuery();
                return true;
            }
        }

        public static bool DeleteStudent(int studentId)
        {

            using (var connection = new SqlConnection(DataAccessSetting.ConnectionString))
            using (var command = new SqlCommand("SP_DeleteStudent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", studentId);

                connection.Open();
                int rowsAffected = (int)command.ExecuteScalar();
                return (rowsAffected == 1);
            }
        }
    }
}
