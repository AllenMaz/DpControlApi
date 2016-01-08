﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Entities;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface ICustomerRepository
    {
        void Add( MCustomer item);
        Task<IEnumerable<MCustomer>> GetAll();
        Task<MCustomer> Find(string customerNo);
        Task Remove(int Id);
        void UpdateById(MCustomer mcustomer);
        Task<IEnumerable<String>> GetCustomerName();
    }
}
