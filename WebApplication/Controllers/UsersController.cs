using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpToTeach.Data;
using HelpToTeach.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpToTeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IRepository<User> _usersRepository = new CouchbaseRepository<User>();

        #region GET
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _usersRepository.GetAll(typeof(User));
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]string id)
        {
            try
            {
                User user = await _usersRepository.Get(id);
                if (user == null) { return NotFound(); }
                return Ok(user);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        //[HttpGet("by_auth0_id/{id}")]
        //public async Task<IActionResult> GetByAuth0Id([FromRoute]string id)
        //{
        //    try
        //    {
        //        List<User> users = await _usersRepository.GetAll(typeof(User));
        //        User user = users.FirstOrDefault(u => u.Auth0Id == id);
        //        if (user == null) { return NotFound(); }
        //        return Ok(user);
        //    }
            //catch (Exception e)
        //    {
        //        return StatusCode(500,e);
        //    }
        //}

        #endregion

        #region POST

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody]User user)
        {
            try
            {
                var result = await _usersRepository.Create(user);
                if (result == null) { return BadRequest(); }
                return Created($"/api/Users/{user.Id}", result);
            }
            catch (Exception e)
            {
                return StatusCode(500,e);
            }
        }

        #endregion

        #region PUT

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody]User user)
        {
            try
            {
                var result = await _usersRepository.Update(user);
                if (result == null) { return BadRequest(result); }
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500,e);
            }
        }

        #endregion

        #region DELETE

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute]string id)
        {
            try
            {
                await _usersRepository.Delete(id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        #endregion


    }
}