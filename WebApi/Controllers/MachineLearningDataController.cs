using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpToTeach.Core.Repository;
using HelpToTeach.Data.Enums;
using HelpToTeach.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineLearningDataController : ControllerBase
    {
        private readonly IMLDataRepository mlDataRepository;
        private readonly IMarkRepository markRepository;
        private readonly ILessonRepository lessonRepository;

        public MachineLearningDataController(
            IMLDataRepository mlDataRepository, 
            IMarkRepository markRepository, 
            ILessonRepository lessonRepository
            )
        {
            this.mlDataRepository = mlDataRepository;
            this.markRepository = markRepository;
            this.lessonRepository = lessonRepository;
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
        public async Task<IActionResult> AddPredictedMarksForFirstMiddle(string groupCourseId, [FromBody] List<Mark> marks)
        {
            try
            {
                var lesson = (await lessonRepository.GetByGroupCourse(groupCourseId)).FirstOrDefault(l => l.LessonType == LessonType.FirstMiddle);
                if (lesson == null) return new NotFoundResult();
                var result = await markRepository.AddPredictedMarks(marks, lesson.Id);
                return new OkObjectResult(result);
            }
            catch
            {
                return new NotFoundResult();
            }
        }

        [HttpGet]
        [Route("secondmiddle")]
        public async Task<IActionResult> GetSecondMiddleData(string groupCourseId)
        {
            var result = await mlDataRepository.GetDataForSecondMiddle(groupCourseId);
            return new JsonResult(result);
        }

        [HttpPost]
        [Route("secondmiddle")]
        public async Task<IActionResult> AddPredictedMarksForSecondMiddle(string groupCourseId, [FromBody] List<Mark> marks)
        {
            try
            {
                var lesson = (await lessonRepository.GetByGroupCourse(groupCourseId)).FirstOrDefault(l => l.LessonType == LessonType.SecondMiddle);
                if (lesson == null) return new NotFoundResult();
                var result = await markRepository.AddPredictedMarks(marks, lesson.Id);
                return new OkObjectResult(result);
            }
            catch
            {
                return new NotFoundResult();
            }
        }

        [HttpGet]
        [Route("final")]
        public async Task<IActionResult> GetFinalData(string groupCourseId)
        {
            var result = await mlDataRepository.GetDataForFinal(groupCourseId);
            return new JsonResult(result);
        }

        [HttpPost]
        [Route("final")]
        public async Task<IActionResult> AddPredictedMarksForFinal(string groupCourseId, [FromBody]List<Mark> marks)
        {
            try
            {
                var lesson = (await lessonRepository.GetByGroupCourse(groupCourseId)).FirstOrDefault(l => l.LessonType == LessonType.Final);
                if (lesson == null) return new NotFoundResult();
                var result = await markRepository.AddPredictedMarks(marks, lesson.Id);
                return new OkObjectResult(result);
            }
            catch
            {
                return new NotFoundResult();
            }
        }
    }
}