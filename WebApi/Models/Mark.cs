using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Mark:EntityBase<Mark>
    {
        public string CourseId { get; set; }
        public int First { get; set; }
        public int Second { get; set; }
        public int FinalMark { get; set; }
        public int Presence { get; set; }
    }
}
