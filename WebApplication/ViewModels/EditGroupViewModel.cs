﻿using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Helpers.Enums;

namespace WebApplication.ViewModels
{
    public class EditGroupViewModel : EditViewModel
    {
        public Group Group { get; set; }
    }
}
