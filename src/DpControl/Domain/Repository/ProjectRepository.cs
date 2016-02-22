using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.IRepository;
using DpControl.Domain.Entities;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;
using DpControl.Domain.Models;
using System.Reflection;
//using Microsoft.Extensions.DependencyInjection;


namespace DpControl.Domain.Repository
{
    public class ProjectRepository : IProjectRepository
    {

        private ShadingContext _context;

        #region Constructors
        public ProjectRepository()
        {
        }

        public ProjectRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }

        #endregion

        public async Task<IEnumerable<MProject>> GetAll()
        {
            var projects = await _context.Projects.Select(c => new MProject
                {
                    ProjectId      =   c.ProjectId,
                    //CustomerName = c.CustomerName,
                    //CustomerNo = c.CustomerNo,
                    ProjectName = c.ProjectName,
                    ProjectNo = c.ProjectNo
                })
                .OrderBy(c => c.ProjectNo)
                .ToListAsync<MProject>();
            return projects;
        }
        public async Task<IEnumerable<MProject>> FindByCustomerNo(string projectNo)
        {
            var customer =  _context.Projects
                        .Where(c => c.ProjectNo == projectNo)
                        .Select(c=> new MProject
                        {
                            ProjectId = c.ProjectId,
                            //CustomerName = c.CustomerName,
                            //CustomerNo = c.CustomerNo,
                            ProjectName = c.ProjectName,
                            ProjectNo = c.ProjectNo
                        });
            if (customer == null)
                throw new KeyNotFoundException();
            return await customer.ToListAsync<MProject>();
        }

        public async Task Add(MProject project)
        {
            if (project == null)
            {
                throw new ArgumentNullException();
            }
            _context.Projects.Add(new Project
            {
                //CustomerName = project.CustomerName,
                //CustomerNo = project.CustomerNo,
                ProjectName = project.ProjectName,
                ProjectNo = project.ProjectNo,
                ModifiedDate = DateTime.Now
            });
            await _context.SaveChangesAsync();
        }
 
        public async Task Update(MProject mcustomer)
        {
            var customer = await _context.Projects.FirstOrDefaultAsync(c => c.ProjectId == mcustomer.ProjectId);
                if (customer == null)
                    throw new KeyNotFoundException();
                //customer.CustomerName = mcustomer.CustomerName;
                //customer.CustomerNo = mcustomer.CustomerNo;
                customer.ProjectName = mcustomer.ProjectName;
                customer.ProjectNo = mcustomer.ProjectNo;
                customer.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task RemoveById(int Id)
        {
            var toDelete = new Project { CustomerId = Id };
            _context.Projects.Attach(toDelete);
            _context.Projects.Remove(toDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<String>> GetProjectName()
        {
            return await _context.Projects.Select(c => c.ProjectName).ToListAsync<String>();
        }

        //public async Task<IEnumerable<MCustomer>> FindRangeByOrder(Query query)
        //{
        //    var customers =  _context.Customers.Select(c => new MCustomer
        //    {
        //        CustomerId = c.CustomerId,
        //        CustomerName = c.CustomerName,
        //        CustomerNo = c.CustomerNo,
        //        ProjectName = c.ProjectName,
        //        ProjectNo = c.ProjectNo
        //    });

        //    if (query.orderby.OrderbyBehavior == "DESC")
        //    {
        //        for(int i = 0; i < query.orderby.OrderbyField.Length; i++)
        //        {
        //            if(typeof(MCustomer).GetProperty)
        //            customers = customers.OrderBy();

        //        }
        //    }
        //    else
        //    {
        //        customers.OrderBy()
        //    }
        //    .OrderBy(c => c.CustomerNo)
        //    .ToListAsync<MCustomer>();
        //    return customers;
        //}

//        Array
    }
}
