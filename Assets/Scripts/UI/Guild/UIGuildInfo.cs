using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildInfo : MonoBehaviour {

    public Text guildName;
    public Text guileID;
    public Text leader;

    public Text notice;

    public Text memberNumber;

    private NGuildInfo info;

    public NGuildInfo Info
    {
        get { return this.info; }
        set { this.info = value;this.UpdateUI(); }
    }

    private void UpdateUI()
    {
        if (this.info == null)
        {
            this.guildName.text = "无";
            this.guileID.text = "ID:0";
            this.leader.text = "会长:无";
            this.notice.text = "";
            this.memberNumber.text = "成员数量： 0/100";
        }
        else
        {
            this.guildName.text = this.Info.GuildName;
            this.guileID.text = "ID:" + this.info.Id;
            this.leader.text = "会长:" + this.info.leaderName;
            this.notice.text = this.info.Notice;
            this.memberNumber.text = string.Format("成员数量： {0}/100", this.info.memberCount);
        }
    }

  
}
