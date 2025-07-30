using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class Quest
    {
        public QuestDefine Define;
        public NQuestInfo Info;
        public Quest() { }

        /// <summary>
        /// 以接受的任务
        /// </summary>
        /// <param name="info"></param>
        public Quest(NQuestInfo info)
        {
            this.Info = info;
            this.Define = DataManager.Instance.Quests[info.QuestId];
        }
        public Quest(QuestDefine define)
        {
            this.Define= define;
            //this.Info = null;
        }
    }
}
