using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentApiDataAccesseLayer;


namespace StudentAPI.Controllers
{
    [Route("api/Students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet("All", Name = "GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudents()
        {
            var studentsList = StudentApiBusinessLayer.Student.GetAllStudents();
            if (studentsList.Count == 0)
            {
                return NotFound("No Students Found");
            }

            return Ok(studentsList);
        }


        [HttpGet("Passed", Name = "GetPassed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetPassedStudents()
        {
            var studentsList = StudentApiBusinessLayer.Student.GetPassedStudents();
            if (studentsList.Count == 0)
            {
                return NotFound("No Students Found");
            }

            return Ok(studentsList);
        }


        [HttpGet("Avg", Name = "GetAverageGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<double>GetAvgGrade()
        {
            return Ok(StudentApiBusinessLayer.Student.GetAvgGrade());
        }


        [HttpGet("{id}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentDTO> GetStudentById(int id)
        {

            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }
            StudentApiBusinessLayer.Student student = StudentApiBusinessLayer.Student.Find(id);

            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            StudentDTO SDTO = student.SDTO;
            return Ok(SDTO);

        }


        [HttpPost(Name = "AdNewStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> AddStudent(StudentDTO newStudentDTO)
        {
            if (newStudentDTO == null || string.IsNullOrEmpty(newStudentDTO.Name) || newStudentDTO.Age < 0 || newStudentDTO.Grade < 0)
            {
                return BadRequest("Invalid student data.");
            }
            StudentApiBusinessLayer.Student student = new StudentApiBusinessLayer.Student(new StudentDTO(newStudentDTO.Id, newStudentDTO.Name, newStudentDTO.Age, newStudentDTO.Grade));
            student.Save();

            newStudentDTO.Id = student.ID;
            return CreatedAtRoute("GetStudentById", new { id = newStudentDTO.Id }, newStudentDTO);

        }



        [HttpDelete("{id}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteStudent(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted");
            }
            if (StudentApiBusinessLayer.Student.DeleteStudent(id))

                return Ok($"deleted.");
            else
                return NotFound($"not found. no rows deleted!");
        }





        [HttpPut("{id}", Name = "Update Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentDTO> UpdateStudent(int id, StudentDTO updatedStudent)
        {
            if (id < 1 || updatedStudent == null || string.IsNullOrEmpty(updatedStudent.Name) || updatedStudent.Age < 0 || updatedStudent.Grade < 0)
            {
                return BadRequest("Invalid student data.");
            }
            StudentApiBusinessLayer.Student student = StudentApiBusinessLayer.Student.Find(id);
            if (student == null)
            {
                return NotFound($"not found.");
            }
            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            student.Grade = updatedStudent.Grade;
            student.Save();
            return Ok(student.SDTO);

        }

    }
    }

