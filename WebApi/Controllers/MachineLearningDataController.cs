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
        private readonly IMLDataRepository mlDataRepository;

        public MachineLearningDataController(IMLDataRepository mlDataRepository)
        {
            this.mlDataRepository = mlDataRepository;
        }

        [HttpGet]
        [Route("firstmiddle")]
        public async Task<IActionResult> GetFirstMiddleData(string groupCourseId)
        {
            var result = await mlDataRepository.GetDataForFirstMiddle(groupCourseId);
            return new JsonResult(result);
        }

        [HttpPost]
        [Route("firstmiddle")]
        public IActionResult AddPredictedMarksForFirstMiddle([FromBody] List<KeyValuePair<string, int>> list)
        {
            return new JsonResult(list);
        }

        [HttpGet]
        [Route("secondmiddle")]
        public async Task<IActionResult> GetSecondMiddleData(string groupCourseId)
        {
            var result = await mlDataRepository.GetDataForSecondMiddle(groupCourseId);
            return new JsonResult(result);
        }

        [HttpGet]
        [Route("final")]
        public async Task<IActionResult> GetFinalData(string groupCourseId)
        {
            var result = await mlDataRepository.GetDataForFinal(groupCourseId);
            return new JsonResult(result);
        }
    }
}