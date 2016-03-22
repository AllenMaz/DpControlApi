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

            //SceneName must be unique
            var checkData = _context.Scenes.Where(s=>s.SceneName == scene.SceneName).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which SceneName equal to '" + scene.SceneName + "' already exist in system");


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

            //SceneName must be unique
            var checkData =await _context.Scenes.Where(s => s.SceneName == scene.SceneName).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which SceneName equal to '" + scene.SceneName + "' already exist in system");


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

        public SceneSearchModel FindById(int sceneId)
        {
            var result = _context.Scenes.Where(v => v.SceneId == sceneId);
            result = result.Include(s => s.SceneSegments);
            result = result.Include(s => s.Groups);

            var scene = result.FirstOrDefault();
            var sceneSearch = SceneOperator.SetSceneSearchModelCascade(scene);

            return sceneSearch;
        }

        public async Task<SceneSearchModel> FindByIdAsync(int sceneId)
        {
            var result = _context.Scenes.Where(v => v.SceneId == sceneId);
            result = result.Include(s => s.SceneSegments);
            result = result.Include(s => s.Groups);

            var scene = await result.FirstOrDefaultAsync();
            var sceneSearch = SceneOperator.SetSceneSearchModelCascade(scene);

            return sceneSearch;
            
        }

        public IEnumerable<SceneSearchModel> GetAll(Query query)
        {
            var queryData = from S in _context.Scenes
                            select S;

            var result = QueryOperate<Scene>.Execute(queryData, query);
            result = ExpandRelatedEntities(result, query.expand);

            //以下执行完后才会去数据库中查询
            var scenes = result.ToList();
            var scenesSearch = SceneOperator.SetSceneSearchModelCascade(scenes);

            return scenesSearch;
        }

        public async Task<IEnumerable<SceneSearchModel>> GetAllAsync(Query query)
        {
            var queryData = from S in _context.Scenes
                            select S;

            var result = QueryOperate<Scene>.Execute(queryData, query);
            result = ExpandRelatedEntities(result,query.expand);

            //以下执行完后才会去数据库中查询
            var scenes = await result.ToListAsync();
            var scenesSearch = SceneOperator.SetSceneSearchModelCascade(scenes);

            return scenesSearch;
        }

        

        public void RemoveById(int sceneId)
        {
            //When delete Scenes,dependent group's foreign key (SceneId) will be SetNull
            var scene = _context.Scenes.Include(c => c.Groups).FirstOrDefault(c => c.SceneId == sceneId);
            if (scene == null)
                throw new ExpectException("Could not find data which SceneId equal to " + sceneId);

            _context.Scenes.Remove(scene);
            _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int sceneId)
        {
            //When delete Scenes,dependent group's foreign key (SceneId) will be SetNull
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

            //SceneName must be unique
            var checkData = _context.Scenes.Where(s => s.SceneName == mScene.SceneName
                                                        && s.SceneId != sceneId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which SceneName '" + mScene.SceneName + "' already exist in system");

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

            //SceneName must be unique
            var checkData = await _context.Scenes.Where(s=>s.SceneName == mScene.SceneName
                                                        && s.SceneId != sceneId).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which SceneName '" + mScene.SceneName + "' already exist in system");


            //Get UserInfo
            var user =await _userInfo.GetUserInfoAsync();

            scene.SceneName = mScene.SceneName;
            scene.Enable = mScene.Enable;
            scene.Modifier = user.UserName;
            scene.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return scene.SceneId;
        }

        /// <summary>
        /// Expand Related Entities
        /// </summary>
        /// <param name="result"></param>
        /// <param name="expandParams"></param>
        /// <returns></returns>
        private IQueryable<Scene> ExpandRelatedEntities(IQueryable<Scene> result, string[] expandParams)
        {
            var needExpandSceneSegments = ExpandOperator.NeedExpand("SceneSegments", expandParams);
            var needExpandGroups = ExpandOperator.NeedExpand("Groups", expandParams);

            if (needExpandSceneSegments)
                result = result.Include(s => s.SceneSegments);
            if (needExpandGroups)
                result = result.Include(s => s.Groups);

            return result;
        }
    }
}
