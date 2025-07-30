using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildApplyItem : ListView.ListViewItem
{
    public Text nickname;
    public Text @class;
    public Text level;

    public NGuildApplyInfo Info;
    public Action closeAction;

    internal void SetItemInfo(NGuildApplyInfo item)
    {
        this.Info = item;
        if (nickname != null) this.nickname.text = this.Info.Name;
        if (@class != null) this.@class.text = this.Info.Class.ToString();
        if (level != null) this.@class.text = this.Info.Level.ToString();
    }

    public void OnAccept()
    {
        MessageBox.Show(string.Format("要通过【{0}】的公会申请吗？", this.Info.Name), "审批", MessageBoxType.Confirm, "同意加入", "取消").OnYes = () =>
           {
               GuildService.Instance.SendGuildJoinApply(true, this.Info);
               if (closeAction != null)
               {
                   closeAction();
               }
           };
    }

    public void OnDecline()
    {
        MessageBox.Show(string.Format("要拒绝【{0}】的公会申请吗？", this.Info.Name), "审批申请", MessageBoxType.Confirm, "拒绝加入", "取消").OnYes = () =>
             {
                 GuildService.Instance.SendGuildJoinApply(false, this.Info);
                 if (closeAction != null)
                 {
                     closeAction();
                 }
             };
    }
}
