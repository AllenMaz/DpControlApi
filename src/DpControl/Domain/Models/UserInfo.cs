﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class UserInfo
    {
        public string UserName { get; set; }

        public List<string> Roles { get; set; }
    }
}
