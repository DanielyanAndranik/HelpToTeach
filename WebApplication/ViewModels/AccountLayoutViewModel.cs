using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class AccountLayoutViewModel
    {
        public string ActiveTab { get; set; }
        public User User { get; set; }
    }
}
