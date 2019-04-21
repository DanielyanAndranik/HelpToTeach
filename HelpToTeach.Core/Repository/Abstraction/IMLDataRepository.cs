using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HelpToTeach.Core.Repository
{
    public interface IMLDataRepository
    {
        Task<List<MiddleMarkData>> GetDataForFirstMiddle(string groupCourseId);
        Task<List<MiddleMarkData>> GetDataForSecondMiddle(string groupCourseId);
        Task<List<MiddleMarkData>> GetDataForFinal(string groupCourseId);
    }
}
