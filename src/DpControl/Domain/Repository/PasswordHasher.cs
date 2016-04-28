using DpControl.Domain.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Repository
{
    public class PasswordHasher: PasswordHasher<ApplicationUser>
    {
    }
}
