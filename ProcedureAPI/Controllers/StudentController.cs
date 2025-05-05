using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ProcedureAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly string _connectionString;

        public StudentController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Student> std = new List<Student>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    std.Add(new Student
                    {
                        Id = (int)reader["Id"],
                        StudentName = reader["StudentName"].ToString(),
                        Age = (int)reader["Age"],
                        Address = reader["Address"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString()
                    });
                }
            }
            return Ok(std);
        }
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {           
            try
            {
                Student student = null;
                using (SqlConnection connect = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("GetStudentById", connect);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    connect.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        student = new Student
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            StudentName = reader["StudentName"].ToString(),
                            Age = Convert.ToInt32(reader["Age"]),
                            Address = reader["Address"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString()
                        };
                    }
                }
                if(student == null)
                {
                    return NotFound(new { Message = "Khong tim thay sinh vien!" });
                }
                return Ok(student);
            }
            catch(Exception e)
            {
                return BadRequest(new { Message = "Loi " + e.Message });
            }
        }
        [HttpPost]
        public IActionResult AddStudent(int Id, string StudentName, int Age, string Address, string PhoneNumber)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("AddStudent", connect);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@StudentName", StudentName);
                    cmd.Parameters.AddWithValue("@Age", Age);
                    cmd.Parameters.AddWithValue("@Address", Address);
                    cmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                    connect.Open();

                    Student newStudent = null;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            newStudent = new Student
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                StudentName = reader["StudentName"].ToString(),
                                Age = Convert.ToInt32(reader["Age"]),
                                Address = reader["Address"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString()
                            };
                        }
                    }
                    return Ok(new
                    {
                        message = "Them sinh vien thanh cong!",
                        student = newStudent
                    });
                }
            }
            catch(Exception e)
            {
                return BadRequest(new { message = "Error: " + e.Message });
            }
        }
    }
}
