using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Models
{
    public class PageResult<T> where T :new()
    {
        public int total { get; set; }

        public List<T> rows { get; set; }

        public PageResult(int totalCount,List<T> pageRows)
        {
            total = totalCount;
            rows = pageRows;
        }
    }
}
