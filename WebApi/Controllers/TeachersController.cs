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
    public class TeachersController : ControllerBase
    {

        private readonly IRepository<User> _teachersRepository;

        public TeachersController()
        {
            
        }

        #region GET

        [HttpGet]
        public async Task<IActionResult> GetAllTeachers()
        {
            try
            {
                var teachers = await _teachersRepository.GetAll(typeof(User));
                return Ok(teachers);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        #endregion

    }
}