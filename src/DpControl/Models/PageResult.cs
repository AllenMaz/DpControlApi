using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Models
{
    public class PageResult<T> where T :new()
    {
        #region BootStrap Table
        public int total { get; set; }

        public List<T> rows { get; set; }

        public PageResult(int totalCount, List<T> pageRows)
        {
            total = totalCount;
            rows = pageRows;
        }
        #endregion

        #region Jquery Table
        public string sEcho { get; set; }

        public int iTotalRecords { get; set; }

        public int iTotalDisplayRecords { get; set; }
        public List<T> aaData { get; set; }

        public PageResult(string secho,int totalDisplayRecords, List<T> pageRows)
        {
            sEcho = secho;
            //iTotalRecords = totalRecords;
            iTotalDisplayRecords = totalDisplayRecords;
            aaData = pageRows;
        }
        #endregion
    }

    public class DataTableReturnObject
    {
        private long iTotalRecords;
        private long iTotalDisplayRecords;
        private int sEcho;
        private String[][] aaData;

        public DataTableReturnObject(long totalRecords, long totalDisplayRecords, int echo, String[][] d)
        {
            iTotalRecords = totalRecords;
            iTotalDisplayRecords = totalDisplayRecords;
            sEcho = echo;
            aaData = d;
        }
        
    }

}
