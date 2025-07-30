﻿using Assets.Scripts.Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services
{
    class StatusService : Singleton<StatusService>, IDisposable
    {
        public delegate bool StatusNotifyHandler(NStatus status);
        Dictionary<StatusType, StatusNotifyHandler> eventMap = new Dictionary<StatusType, StatusNotifyHandler>();
        HashSet<StatusNotifyHandler> handlers = new HashSet<StatusNotifyHandler>();
        public StatusService()
        {
            MessageDistributer.Instance.Subscribe<StatusNotify>(this.OnStatusNotify);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<StatusNotify>(this.OnStatusNotify);
        }
        public void Init()
        {

        }
        public void RegistorStatusNofity(StatusType function,StatusNotifyHandler action)
        {
            if (handlers.Contains(action)) return;
            if (!eventMap.ContainsKey(function))
            {
                eventMap[function] = action;
            }
            else
                eventMap[function] += action;
            handlers.Add(action);
        }
     

        public void OnStatusNotify(object sender, StatusNotify notify)
        {
            foreach (NStatus status in notify.Status)
            {
                Notify(status);
            }
        }
        private void Notify(NStatus status)
        {
            Debug.LogFormat("StatusNotify:[{0}] [{1}] {2}:{3}", status.Type, status.Action, status.Id, status.Value);
            if (status.Type == StatusType.Money)
            {
                if (status.Action == StatusAction.Add)
                    User.Instance.AddGold(status.Value);
                else if (status.Action == StatusAction.Delete)
                    User.Instance.AddGold(-status.Value);
                ShopManager.Instance.RefreshShopMoney();
            }
            StatusNotifyHandler handler;
            if (eventMap.TryGetValue(status.Type, out handler))
            {
                handler(status);
            }
        }
    }
}
