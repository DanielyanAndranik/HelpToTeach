using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HelpToTeach.Core.Repository
{
    public interface IMLDataRepository
    {
        Task<List<MiddleFeatures>> GetDataForFirstMiddle(string groupCourseId);
        Task<List<MiddleFeatures>> GetDataForSecondMiddle(string groupCourseId);
        Task<List<MiddleFeatures>> GetDataForFinal(string groupCourseId);
    }
}
