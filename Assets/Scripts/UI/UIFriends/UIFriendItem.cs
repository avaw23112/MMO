using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFriendItem : ListView.ListViewItem
{

    public ListView Owner;
    public Text nickName;
    public Text Class;
    public Text Level;
    public Text status;
    public Image backGround;

    public Sprite NormalBg;
    public Sprite SelectedBg;

    public override void onSelected(bool selecteed)
    {
        this.backGround.overrideSprite = selecteed ? SelectedBg : NormalBg;
    }
    public NFriendInfo Info;
    public void SetFriendInfo(NFriendInfo item)
    {
        this.Info = item;
        if(this.nickName != null ) { this.nickName.text = this.Info.friendInfo.Name; }
        if(this.Class != null ) { this.Class.text = this.Info.friendInfo.Class.ToString(); }
        if(this.Level != null ) { this.Level.text = this.Info.friendInfo.Level.ToString(); }
        if(this.status != null ) { this.status.text = this.Info.Status == 1?"在线":"离线"; }
    }
}
