using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.IRepository;
using DpControl.Domain.Entities;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;
using DpControl.Domain.Execptions;

namespace DpControl.Domain.Repository
{
    public class SceneRepository : ISceneRepository
    {
        ShadingContext _context;
        private readonly IUserInfoRepository _userInfo;

        #region Constructors
        public SceneRepository()
        {
        }

        public SceneRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }
        public SceneRepository(ShadingContext dbContext, IUserInfoRepository userInfo)
        {
            _context = dbContext;
            _userInfo = userInfo;
        }
        #endregion

        public int Add(SceneAddModel scene)
        {
            var project = _context.Projects.FirstOrDefault(c => c.ProjectId == scene.ProjectId);
            if (project == null)
                throw new ExpectException("Could not find Project data which ProjectId equal to " + scene.ProjectId);

            //Get UserInfo
            var user = _userInfo.GetUserInfo();

            var model = new Scene
            {
                ProjectId = scene.ProjectId,
                SceneName = scene.SceneName,
                Enable = scene.Enable,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };
            _context.Scenes.Add(model);
            _context.SaveChanges();
            return model.SceneId;
        }

        public async Task<int> AddAsync(SceneAddModel scene)
        {
            var project = _context.Projects.FirstOrDefault(c => c.ProjectId == scene.ProjectId);
            if (project == null)
                throw new ExpectException("Could not find Project data which ProjectId equal to " + scene.ProjectId);

            //Get UserInfo
            var user = await _userInfo.GetUserInfoAsync();
            var model = new Scene
            {
                ProjectId = scene.ProjectId,
                SceneName = scene.SceneName,
                Enable = scene.Enable,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };
            _context.Scenes.Add(model);
            await _context.SaveChangesAsync();
            return model.SceneId;
        }

        public SceneSearchMoodel FindById(int sceneId)
        {
            var scene = _context.Scenes.Where(v => v.SceneId == sceneId)
                .Select(v => new SceneSearchMoodel()
                {
                    SceneId = v.SceneId,
                    SceneName = v.SceneName,
                    ProjectId = v.ProjectId,
                    Enable = v.Enable,
                    Creator = v.Creator,
                    CreateDate = v.CreateDate.ToString(),
                    Modifier = v.Modifier,
                    ModifiedDate = v.ModifiedDate.ToString()
                }).FirstOrDefault();

            return scene;
        }

        public async Task<SceneSearchMoodel> FindByIdAsync(int sceneId)
        {
            var scene =await _context.Scenes.Where(v => v.SceneId == sceneId)
                .Select(v => new SceneSearchMoodel()
                {
                    SceneId = v.SceneId,
                    SceneName = v.SceneName,
                    ProjectId = v.ProjectId,
                    Enable = v.Enable,
                    Creator = v.Creator,
                    CreateDate = v.CreateDate.ToString(),
                    Modifier = v.Modifier,
                    ModifiedDate = v.ModifiedDate.ToString()
                }).FirstOrDefaultAsync();

            return scene;
        }

        public IEnumerable<SceneSearchMoodel> GetAll(Query query)
        {
            var queryData = from S in _context.Scenes
                            select S;

            var result = QueryOperate<Scene>.Execute(queryData, query);

            //以下执行完后才会去数据库中查询
            var scenes = result.ToList();

            var scenesSearch = scenes.Select(v => new SceneSearchMoodel
            {
                SceneId = v.SceneId,
                SceneName = v.SceneName,
                ProjectId = v.ProjectId,
                Enable = v.Enable,
                Creator = v.Creator,
                CreateDate = v.CreateDate.ToString(),
                Modifier = v.Modifier,
                ModifiedDate = v.ModifiedDate.ToString()
            });

            return scenesSearch;
        }

        public async Task<IEnumerable<SceneSearchMoodel>> GetAllAsync(Query query)
        {
            var queryData = from S in _context.Scenes
                            select S;

            var result = QueryOperate<Scene>.Execute(queryData, query);

            //以下执行完后才会去数据库中查询
            var scenes = await result.ToListAsync();

            var scenesSearch = scenes.Select(v => new SceneSearchMoodel
            {
                SceneId = v.SceneId,
                SceneName = v.SceneName,
                ProjectId = v.ProjectId,
                Enable = v.Enable,
                Creator = v.Creator,
                CreateDate = v.CreateDate.ToString(),
                Modifier = v.Modifier,
                ModifiedDate = v.ModifiedDate.ToString()
            });

            return scenesSearch;
        }

        public void RemoveById(int sceneId)
        {
            var scene = _context.Scenes.Include(c => c.Groups).FirstOrDefault(c => c.SceneId == sceneId);
            if (scene == null)
                throw new ExpectException("Could not find data which SceneId equal to " + sceneId);

            _context.Scenes.Remove(scene);
            _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int sceneId)
        {
            var scene = _context.Scenes.Include(c=>c.Groups).FirstOrDefault(c => c.SceneId == sceneId);
            if (scene == null)
                throw new ExpectException("Could not find data which SceneId equal to " + sceneId);

            _context.Remove(scene);
            await _context.SaveChangesAsync();
        }

        public int UpdateById(int sceneId, SceneUpdateModel mScene)
        {
            var scene = _context.Scenes.FirstOrDefault(c => c.SceneId == sceneId);
            if (scene == null)
                throw new ExpectException("Could not find data which SceneId equal to " + sceneId);

            //Get UserInfo
            var user = _userInfo.GetUserInfo();

            scene.SceneName = mScene.SceneName;
            scene.Enable = mScene.Enable;
            scene.Modifier = user.UserName;
            scene.ModifiedDate = DateTime.Now;

            _context.SaveChanges();
            return scene.SceneId;
        }

        public async Task<int> UpdateByIdAsync(int sceneId, SceneUpdateModel mScene)
        {
            var scene = _context.Scenes.FirstOrDefault(c => c.SceneId == sceneId);
            if (scene == null)
                throw new ExpectException("Could not find data which SceneId equal to " + sceneId);

            //Get UserInfo
            var user =await _userInfo.GetUserInfoAsync();

            scene.SceneName = mScene.SceneName;
            scene.Enable = mScene.Enable;
            scene.Modifier = user.UserName;
            scene.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return scene.SceneId;
        }
        

    }
}
