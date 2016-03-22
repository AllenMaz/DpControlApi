using DpControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DpControl.Domain.Models
{
    public class CustomerBaseModel
    {
        [Required(ErrorMessage = "CustomerName is required!")]
        [MaxLength(50, ErrorMessage = "CustomerName must be less than 50 characters!")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "CustomerNo is required!")]
        [MaxLength(50, ErrorMessage = "CustomerNo must be less than 50 characters!")]
        public string CustomerNo { get; set; }
    }

    public class CustomerAddModel: CustomerBaseModel
    {
        
        
    }

    public class CustomerUpdateModel: CustomerBaseModel
    {
    }

    public class CustomerSearchModel: CustomerBaseModel
    {
        public int CustomerId { get; set; }
        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public IEnumerable<ProjectSearchModel> Projects { get; set; }
    }

    public static class CustomerOperator
    {
        /// <summary>
        /// Cascade set CustomerSearchModel Results
        /// </summary>
        public static IEnumerable<CustomerSearchModel> SetCustomerSearchModelCascade(List<Customer> customers)
        {
            var customerSearchModels = customers.Select(c => SetCustomerSearchModelCascade(c));

            return customerSearchModels;
        }

        /// <summary>
        /// Cascade set CustomerSearchModel Result
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static CustomerSearchModel SetCustomerSearchModelCascade(Customer customer)
        {
            if (customer == null) return null;
            var customerSearchModel = new CustomerSearchModel
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                CustomerNo = customer.CustomerNo,
                Creator = customer.Creator,
                CreateDate = customer.CreateDate,
                Modifier = customer.Modifier,
                ModifiedDate = customer.ModifiedDate,
                Projects = ProjectOperator.SetProjectSearchModelCascade(customer.Projects)
            };

            return customerSearchModel;
        }
    }
}
