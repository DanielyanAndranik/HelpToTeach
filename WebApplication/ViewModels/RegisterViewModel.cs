using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Helpers.Enums;

namespace WebApplication.ViewModels
{
    public class RegisterViewModel
    {
        public RegisterMode Mode { get; set; }

        public RegisterViewModel(RegisterMode mode)
        {
            Mode = mode;
        }
    }
}
