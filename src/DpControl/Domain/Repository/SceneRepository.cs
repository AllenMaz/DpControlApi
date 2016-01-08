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

            int _customerId;

            // get groups with projectNo = projectNo
            var query = await GetCustomerByProjectNo(projectNo);

            _customerId = query.CustomerId;

            // does the Name exist?
            if (query.Scenes.Select(s => s.Name).Contains(sceneName))
            {
                throw new Exception("The group already exist.");
            }

            // create new Group
            Scene _scene = new Scene
            {
                Name = sceneName,
                Enable = false,
                ModifiedDate = DateTime.Now,
                CustomerId = _customerId
            };
            _context.Scenes.Add(_scene);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<MScene>> GetAll(string projectNo)
        {
            // get groups by the projectNo
            var query = await GetCustomerByProjectNo(projectNo);

            return query.Scenes.Select(s => new MScene
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
                throw new Exception("The group does not exist.");
            }

            var toDelete = new Scene { SceneId = Id };
            _context.Scenes.Attach(toDelete);
            _context.Scenes.Remove(toDelete);
            await _context.SaveChangesAsync();

        }
        public async void UpdateById(MScene mScene, string projectNo)
        {
            // get groups by the projectNo
            var query = await GetCustomerByProjectNo(projectNo);

            var _single = query.Scenes.Where(s => s.SceneId == mScene.SceneId).Single();
            if (_single == null)
            {
                throw new KeyNotFoundException();
            }
            _single.Name = mScene.Name;
            _context.Scenes.Update(_single);
            await _context.SaveChangesAsync();
        }

        async Task<Customer> GetCustomerByProjectNo(string projectNo)
        {
            var query = await _context.Customers
                        .Include(c => c.Scenes)
                        .Where(c => c.ProjectNo == projectNo)
                        .SingleAsync();
            if (query == null)
            {
                throw new KeyNotFoundException();
            }
            return query;
        }
    }
}
