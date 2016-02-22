using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.IRepository;
using DpControl.Domain.Entities;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;

namespace DpControl.Domain.Repository
{
    public class SceneRepository : ISceneRepository
    {
        ShadingContext _context;

        #region Constructors
        public SceneRepository()
        {
        }

        public SceneRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }
        #endregion

        public async  Task Add(string sceneName, string projectNo)
        {
            if (string.IsNullOrEmpty(sceneName) || string.IsNullOrEmpty(projectNo))
            {
                throw new ArgumentNullException();
            }

            // get projectNo from Customer
            var _customer = await _context.Customers
                .Include(c => c.Scenes)
                .Where(c => c.ProjectNo == projectNo)
                .SingleAsync();

            // create new Group
            _context.Scenes.Add(new Scene
            {
                Name = sceneName,
                Enable = false,
                ModifiedDate = DateTime.Now,
                CustomerId = _customer.CustomerId
            });
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<MScene>> GetAll(string projectNo)
        {
            if (string.IsNullOrWhiteSpace(projectNo))
            {
                throw new ArgumentNullException();
            }

            // get projectNo from Customer
            var _customer = await _context.Customers
                .Include(c => c.Scenes)
                .Where(c => c.ProjectNo == projectNo)
                .SingleAsync();

            return _customer.Scenes.Select(s => new MScene
            {
                SceneId = s.SceneId,
                Name = s.Name
            })
            .ToList<MScene>();
        }

        public async Task Remove(int Id)
        {
            if (Id == 0)
            {
                throw new ArgumentNullException();
            }

            var toDelete = new Scene { SceneId = Id };
            _context.Scenes.Attach(toDelete);

            //remove data in related table - Groups - optional relationship with data undeleted (set to Null), just load data into memory
            _context.Groups.Where(l => l.SceneId == Id).Load();

            // Scensegments will be deleted by Cascade setting in database
            _context.Scenes.Remove(toDelete);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateById(MScene mScene, string projectNo)
        {
            // get projectNo from Customer
            var _customer = await _context.Customers
                .Include(c => c.Scenes)
                .Where(c => c.ProjectNo == projectNo)
                .SingleAsync();

            var _single = _customer.Scenes.Where(s => s.SceneId == mScene.SceneId).Single();
            _single.Name = mScene.Name;
            await _context.SaveChangesAsync();
        }

        //async Task<Customer> GetCustomerByProjectNo(string projectNo)
        //{
        //    var query = await _context.Customers
        //                .Include(c => c.Scenes)
        //                .Where(c => c.ProjectNo == projectNo)
        //                .SingleAsync();
        //    if (query == null)
        //    {
        //        throw new KeyNotFoundException();
        //    }
        //    return query;
        //}
    }
}
