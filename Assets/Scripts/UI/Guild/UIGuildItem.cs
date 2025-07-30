using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildItem : ListView.ListViewItem
{
    public NGuildInfo Info;
    public Text Id;
    public Text Name;
    public Text leader;
    public Text quantity;


    public void SetGuildInfo(NGuildInfo item)
    {
        Info = new NGuildInfo();
        Info = item;
        if (this.Id != null) { this.Id.text = Info.Id.ToString(); }
        if (this.Name != null) { this.Name.text = Info.GuildName; }
        if (this.leader != null) { this.leader.text = Info.leaderName; }
        if (this.quantity != null) { this.quantity.text = Info.memberCount.ToString(); }
    }
}


