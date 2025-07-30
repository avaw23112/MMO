using Entities;
using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace Managers
{
    class CharacterManager : Singleton<CharacterManager>, IDisposable
    {
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();


        public UnityAction<Character> OnCharacterEnter;
        public UnityAction<Character> OnCharacterLeave;
        public CharacterManager()
        {

        }

        public void Dispose()
        {
        }

        public void Init()
        {

        }

        public void Clear()
        {
            int[] keys = this.Characters.Keys.ToArray();
            foreach (var key in keys)
            {
                this.RemoveCharacter(key);
            }

            this.Characters.Clear();
        }

        public void AddCharacter(NCharacterInfo characterInfo)
        {
            UnityEngine.Debug.LogFormat("AddCharacter:{0}:{1} Map:{2} Entity:{3}", characterInfo.Id, characterInfo.Name, characterInfo.mapId, characterInfo.Entity.String());
            Character character = new Character(characterInfo);
            this.Characters[character.entityId] = character;
            EntityManager.Instance.AddEntity(character);
            if (OnCharacterEnter!=null)
            {
                OnCharacterEnter(character);
            }
        }


        public void RemoveCharacter(int entityId)
        {
            UnityEngine.Debug.LogFormat("RemoveCharacter:{0}", entityId);
            //判断要删除的角色是否存在
            if (this.Characters.ContainsKey(entityId)) {
                //在实体管理器中移除 
                EntityManager.Instance.RemoveEntity(this.Characters[entityId].Info.Entity);
                //执行角色离开事件
                if (OnCharacterLeave != null) {

                    OnCharacterLeave(this.Characters[entityId]);
                }
            }
            //将角色列表中的角色删除
            this.Characters.Remove(entityId);
        }
        public Character GetCharacter(int characterId) 
        {
            Character character;
            this.Characters.TryGetValue(characterId, out character);
            return character;
        }
    }
}
