using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.IRepository;
using DpControl.Domain.Entities;
using DpControl.Domain.EFContext;

namespace DpControl.Domain.Repository
{
    public class GroupRepository : IGroupRepository
    {
        ShadingContext _context;
        public GroupRepository()
        {

        }
        public GroupRepository(ShadingContext context)
        {
            _context = context;
        }

        void Add(MGroup item, string ProjectNo)
        {
            Group group = new Group
            {
                
            };
        }
    }
}
