using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpToTeach.Core.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineLearningDataController : ControllerBase
    {
        private readonly IMarkRepository markRepository;
        private readonly IStudentRepository studentRepository;

        public MachineLearningDataController(IMarkRepository markRepository, IStudentRepository studentRepository)
        {
            this.markRepository = markRepository;
            this.studentRepository = studentRepository;
        }

        [HttpGet]
        [Route("firstmiddle")]
        public IActionResult GetFirstMiddleData(string groupCourseId)
        {
            return null;
        }

        [HttpGet]
        [Route("secondmiddle")]
        public IActionResult GetSecondMiddleData()
        {
            return null;
        }

        [HttpGet]
        [Route("final")]
        public IActionResult GetFinalData()
        {
            return null;
        }
    }
}