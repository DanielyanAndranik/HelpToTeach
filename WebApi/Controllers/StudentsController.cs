using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpToTeach.Core.Repository;
using HelpToTeach.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        private readonly IRepository<Student> _studentsRepository;
        #region GET

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var students = await _studentsRepository.GetAll(typeof(Student));
                return Ok(students);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        #endregion

    }
}