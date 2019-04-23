using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HelpToTeach.Core.Repository
{
    public interface IMLDataRepository
    {
        Task<List<MiddleMarkFeatures>> GetDataForFirstMiddle(string groupCourseId);
        Task<KeyValuePair<bool, List<Mark>>> GetFirstMiddlePrediction(string groupCourseId);
        Task<List<MiddleMarkFeatures>> GetDataForSecondMiddle(string groupCourseId);
        Task<KeyValuePair<bool, List<Mark>>> GetSecondMiddlePrediction(string groupCourseId);
        Task<List<FinalMarkFeatures>> GetDataForFinal(string groupCourseId);
        Task<KeyValuePair<bool, List<Mark>>> GetFinalPrediction(string groupCourseId);
    }
}
