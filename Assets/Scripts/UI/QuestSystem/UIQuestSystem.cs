
using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIQuestSystem : UIWindow
{
    public Text title;
    public GameObject itemPrefab;

    public TabView Tabs;
    public ListView listMain;
    public ListView listBranch;

    public UIQuestInfo questInfo;

    private bool showAvailableList = false;


    void Start()
    {
        this.listMain.onItemSelected += this.OnQuestSelected;
        this.listBranch.onItemSelected += this.OnQuestSelected;
        this.Tabs.OnTabSelect += OnSelectTab;
        RefreshUI();
    }

    void OnSelectTab(int idx)
    {
        showAvailableList = idx == 1;
        RefreshUI();
    }
    private void RefreshUI()
    {
        ClearAllQuestList();
        InitAllQuestItems();
    }
    /// <summary>
    /// 初始化所有任务
    /// </summary>
    void InitAllQuestItems()
    {
        foreach (var kv in QuestManager.Instance.allQuests)
        {
            if (showAvailableList)
            {
                if (kv.Value.Info != null)
                    continue;
            }
            else
            {
                if (kv.Value.Info == null)
                    continue;
            }
            GameObject go = Instantiate(itemPrefab, kv.Value.Define.Type == QuestType.Main ? this.listMain.transform : this.listBranch.transform);
            UIQuestItem ui = go.GetComponent<UIQuestItem>();
            ui.SetQuestInfo(kv.Value);
            //if(kv.Value.Define.Type == QuestType.Main)
            //{
            //    //UIQuestItem ui = listMain.AddItem<UIQuestItem>(itemPrefab, kv.Value.Define.Type == QuestType.Main ? this.listMain.transform : this.listBranch.transform);
            //    //ui.SetQuestInfo(kv.Value);
            //    this.listMain.AddItem(ui as ListView.ListViewItem);
            //}
            //else
            //{
            //    //UIQuestItem ui = listMain.AddItem<UIQuestItem>(itemPrefab,this.listBranch.transform);
            //    this.listBranch.AddItem(ui as ListView.ListViewItem);
            //}
            this.listMain.AddItem(ui as ListView.ListViewItem);
        }
    }

    void ClearAllQuestList()
    {
        listMain.RemoveAll();
        listBranch.RemoveAll();
    }
    public void OnQuestSelected(ListView.ListViewItem item)
    {
        UIQuestItem questItem = (UIQuestItem)item;
        this.questInfo.SetQuestInfo(questItem.quest);
    }
}
