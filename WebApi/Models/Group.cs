using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Group:EntityBase<Group>
    {
        public string Name { get; set; }
    }
}
