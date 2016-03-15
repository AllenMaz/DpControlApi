using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Models
{
    public class BaseModel
    {
        #region BootStrap Tables
        //public int limit { get; set; }
        ////偏移量
        //public int offset { get; set; }
        #endregion
    }
    
    public class JQueryDataTableParams
    {
        /// <summary>  
        /// Request sequence number sent by DataTable,  
        /// same value must be returned in response  
        /// </summary>      
        public string sEcho { get; set; }
        /// <summary>  
        /// Text used for filtering  
        /// </summary>  
        public string sSearch { get; set; }
        /// <summary>  
        /// Number of records that should be shown in table  
        /// </summary>  
        public int iDisplayLength { get; set; }
        /// <summary>  
        /// First record that should be shown(used for paging)  
        /// </summary>  
        public int iDisplayStart { get; set; }
        /// <summary>  
        /// Number of columns in table  
        /// </summary>  
        public int iColumns { get; set; }
        /// <summary>  
        /// Number of columns that are used in sorting  
        /// </summary>  
        public int iSortingCols { get; set; }
        /// <summary>  
        /// Comma separated list of column names  
        /// </summary>  
        public string sColumns { get; set; }

    }

    public class Menu
    {
        public string MenuId { get; set; }

        public string MenuName { get; set; }

        public int Order { get; set; }

        /// <summary>
        /// fullname eg:logo.png
        /// </summary>
        public string MenuIcon { get; set; }

        public string MenuUrl { get; set; }

        public List<Menu> SecondaryMenus { get; set; }


        public Menu()
        {
            SecondaryMenus = new List<Menu>();
        }
    }
}
