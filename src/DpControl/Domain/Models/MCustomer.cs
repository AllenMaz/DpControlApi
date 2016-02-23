using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class CustomerAddModel
    {
        [Required(ErrorMessage ="CustomerName is required!")]
        [MaxLength(50,ErrorMessage = "CustomerName must be less than 50 characters!")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "CustomerNo is required!")]
        [MaxLength(50, ErrorMessage = "CustomerNo must be less than 50 characters!")]
        public string CustomerNo { get; set; }
        
    }

    public class CustomerUpdateModel:CustomerAddModel
    {
        public int CustomerId { get; set; }
    }

    public class CustomerSearchModel:CustomerAddModel
    {
        public int CustomerId { get; set; }
        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }


}
