using System;
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
    public class CoursesController : ControllerBase
    {
        private readonly IRepository<Course> _coursesRepository = new CouchbaseRepository<Course>();

        #region GET

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            try
            {
                var courses = await _coursesRepository.GetAll(typeof(Course));
                return Ok(courses);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById([FromRoute]string id)
        {
            try
            {
                var courses = await _coursesRepository.GetAll(typeof(Course));
                var course = courses.FirstOrDefault(s => s.Id == id);
                if (course == null)
                {
                    return NotFound();
                }
                return Ok(course);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        #endregion

        #region POST

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]Course course)
        {
            try
            {
                var result = await _coursesRepository.Create(course);
                if (result == null)
                {
                    return BadRequest(course);
                }
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        #endregion

        #region PUT

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody]Course course)
        {
            try
            {
                var result = await _coursesRepository.Update(course);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        #endregion

        #region DELETE

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody]Course course)
        {
            try
            {
                await _coursesRepository.Delete(course.Id);
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        #endregion

    }
}