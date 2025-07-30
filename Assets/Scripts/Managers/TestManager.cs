using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;

namespace Managers
{
    public class TestManager : Singleton<TestManager>
    {

        public void Init()
        {
            NPCManager.Instance.RegisterNpcEvent(Common.Data.NpcFunction.InvokeShop, OnNpcInvokeShop);
            NPCManager.Instance.RegisterNpcEvent(Common.Data.NpcFunction.InvokeInsrance, OnNpcInvokeInsrance);
        }

        private bool OnNpcInvokeShop(NpcDefine npc)
        {
            Debug.Log("点击了NPC----超市");
            UITest test = UIManager.Instance.Show<UITest>();
            return true;
        }

        private bool OnNpcInvokeInsrance(NpcDefine npc)
        {
            Debug.Log("点击了NPC-----对话");
            MessageBox.Show("点击了NPC" + npc.Name,"NPC对话");
            return true;
        }
    }
}


