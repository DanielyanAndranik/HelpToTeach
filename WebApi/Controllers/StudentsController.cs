﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        private readonly IRepository<Student> _studentsRepository = new CouchbaseRepository<Student>();


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
                return StatusCode(500,e);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById([FromRoute]string id) {
            try
            {
                var students = await _studentsRepository.GetAll(typeof(Student));
                var student = students.FirstOrDefault(s => s.Id == id);
                if (student == null) {
                    return NotFound();
                }
                return Ok(student);
            }
            catch (Exception e)
            {
                return StatusCode(500,e);
            }
        }

        #endregion

        #region POST

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]Student student)
        {
            try
            {
                var result = await _studentsRepository.Create(student);
                if (result == null)
                {
                    return BadRequest(student);
                }
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500,e);
            }
        }

        #endregion

        #region PUT

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody]Student student)
        {
            try
            {
                var result = await _studentsRepository.Update(student);
                if (result == null) {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500,e);
            }
        }

        #endregion

        #region DELETE

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody]Student student)
        {
            try
            {
                await _studentsRepository.Delete(student.Id);
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500,e);
            }
        }

        #endregion

    }
}