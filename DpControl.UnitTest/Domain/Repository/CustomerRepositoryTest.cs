using DpControl.Domain.Execptions;
using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DpControl.UnitTest.Domain.Repository
{
    public class CustomerRepositoryTest
    {
        [FromServices]
        public ICustomerRepository _customerRepository { get; set; }
        
    }
}
