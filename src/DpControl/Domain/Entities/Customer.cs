﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNo { get; set; }
        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual List<Project> Projects { get; set; }
        public virtual List<ApplicationUser> Users { get; set; }

        public Customer()
        {
            this.Projects = new List<Project>();
            this.Users = new List<ApplicationUser>();
        }
    }
}
