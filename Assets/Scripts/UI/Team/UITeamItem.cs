using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITeamItem : ListView.ListViewItem
{

    public Text nickName;
    public Image classIcon;
    public Image LeaderIcon;
    public Image Background;
    public int idx;

    public override void onSelected(bool selected)
    {
        this.Background.enabled = selected?true:false;
    }
    public NCharacterInfo Info;


    // Start is called before the first frame update
    void Start()
    {
        this.Background.enabled=false;
    }
    public void SetMemberInfo(int idx, NCharacterInfo item, bool isLeader)
    {
        this.idx = idx;
        this.Info = item;
        if (this.nickName != null) { this.nickName.text = this.Info.Level.ToString().PadRight(4) + this.Info.Name; }
        if (this.classIcon != null) { this.classIcon.overrideSprite = SpriteManager.Instance.ClassIcons[(int)this.Info.Class - 1]; }
        if (this.LeaderIcon != null) { this.LeaderIcon.gameObject.SetActive(isLeader); }
    }
}
