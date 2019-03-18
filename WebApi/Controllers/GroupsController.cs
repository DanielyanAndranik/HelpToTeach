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
    public class GroupsController : ControllerBase
    {
        private readonly IRepository<Group> _groupsRepository = new CouchbaseRepository<Group>();

        #region GET

        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            try
            {
                var groups = await _groupsRepository.GetAll(typeof(Group));
                return Ok(groups);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById([FromRoute]string id)
        {
            try
            {
                var groups = await _groupsRepository.GetAll(typeof(Group));
                var group = groups.FirstOrDefault(s => s.Id == id);
                if (group == null)
                {
                    return NotFound();
                }
                return Ok(group);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        #endregion

        #region POST

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]Group group)
        {
            try
            {
                var result = await _groupsRepository.Create(group);
                if (result == null)
                {
                    return BadRequest(group);
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
        public async Task<IActionResult> Update([FromBody]Group group)
        {
            try
            {
                var result = await _groupsRepository.Update(group);
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
        public async Task<IActionResult> Delete([FromBody]Group group)
        {
            try
            {
                await _groupsRepository.Delete(group.Id);
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