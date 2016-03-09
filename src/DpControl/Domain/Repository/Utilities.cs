using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;

namespace DpControl.Domain.Repository
{
    public class Utilities
    {
        static ShadingContext _context;
        public Utilities()
        {

        }
        public Utilities(ShadingContext context)
        {
            _context = context;
        }

        public static async Task<int> GetLocationIdByProjectNo(string projectNo)
        {
            var project = await _context.Projects.SingleAsync(c => c.ProjectNo == projectNo);
            if (project == null)
                throw new NullReferenceException();
            return 1;
        }
    }
}
