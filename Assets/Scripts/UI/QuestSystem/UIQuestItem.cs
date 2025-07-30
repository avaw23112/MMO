using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestItem : ListView.ListViewItem
{
    public Text title;

    public Image backround;
    public Sprite normalBg;
    public Sprite selectedBg;
    public override void onSelected(bool selected)
    {
        this.backround.overrideSprite = selected ? selectedBg : normalBg;
    }

    public Quest quest;
    public void SetQuestInfo(Quest item)
    {
        this.quest = item;
        if(this.title!=null) this.title.text = this.quest.Define.Name;
    }

    
}
