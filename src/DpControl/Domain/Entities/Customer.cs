using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectNo { get; set; }
        #region relationship
        public virtual List<Operator> Operators { get; set; }
        public virtual List<Location> DeviceLocations { get; set; }           // Devices contained in a company accessed through Locations
        public virtual List<Group> Groups { get; set; }
        public virtual List<Scene> Scenes { get; set; }
        public virtual List<Holiday> Holidays { get; set; }
        #endregion

        public DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }

        public Customer()
        {
            this.Operators = new List<Operator>();
            this.DeviceLocations = new List<Location>();
            this.Groups = new List<Group>();
            this.Scenes = new List<Scene>();
            this.Holidays = new List<Holiday>();
        }
    }
}
