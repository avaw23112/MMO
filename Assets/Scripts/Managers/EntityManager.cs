using Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{

    interface IEntityNotify
    {
        //实例移除通知
        void OnEntityRemoved();
        //同时数据改变通知
        void OnEntityChanged(Entity entity);
        //实体的状态通知
        void OnEntityEvent(EntityEvent entity,int param);

    }
    class EntityManager : Singleton<EntityManager>
    {
        //记录所有实体
        Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        //实体的通知消息
        Dictionary<int, IEntityNotify> notifiers = new Dictionary<int, IEntityNotify>();

        public void RegisterEntityChangerNotify(int entityId, IEntityNotify notfy)
        {
            this.notifiers[entityId] = notfy;
        }

        public void AddEntity(Entity entity)
        {
            entities[entity.entityId] = entity;
        }

        public void RemoveEntity(NEntity entity)
        {
            this.entities.Remove(entity.Id);
            if (notifiers.ContainsKey(entity.Id))
            {
                // 删除Entity游戏对象以及血条
                notifiers[entity.Id].OnEntityRemoved();
                notifiers.Remove(entity.Id);
            }
        }


        internal Entity GetEntity(int entityId)
        {
            Entity entity = null;
            entities.TryGetValue(entityId, out entity);
            return entity;
        }

        internal void OnEntitySync(NEntitySync data)
        {
            Entity entity = null;
            entities.TryGetValue(data.Id, out entity);
            if (entity != null)
            {
                if (data.Entity != null)
                    entity.EntityData = data.Entity;

                if (notifiers.ContainsKey(data.Id))
                {
                    notifiers[entity.entityId].OnEntityChanged(entity);//通知 entity 数据变了

                    notifiers[entity.entityId].OnEntityEvent(data.Event,data.Param);//通知 entity 事件改变了
                }
            }
        }
    }
}
