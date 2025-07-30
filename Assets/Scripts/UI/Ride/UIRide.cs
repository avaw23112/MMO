using Assets.Scripts.Managers;
using Managers;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRide : UIWindow {

    public Text descript;
    public GameObject itemPrefab;
    public ListView listMain;
    private UIRideItem selectedItem;

	void Start () {
        RefreshUI();
        this.listMain.onItemSelected += this.OnItemSelected;
	}

    private void OnDestroy()
    {
        this.listMain.onItemSelected -= this.OnItemSelected;
    }

    private void OnItemSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIRideItem;
        this.descript.text = this.selectedItem.item.Define.Description;
    }


    public void RefreshUI()
    {
        ClearItems();
        InitItems();
    }

    private void ClearItems()
    {
        this.listMain.RemoveAll();
    }

    private void InitItems()
    {
        foreach (var kv in ItemManager.Instance.Items)
        {
            if (kv.Value.Define.Type == ItemType.Ride && 
                (kv.Value.Define.LimitClass == CharacterClass.None || 
                kv.Value.Define.LimitClass == User.Instance.CurrentCharacter.Class))
            {
                if (EquipManager.Instance.Contains(kv.Key))
                    continue;
                GameObject go = Instantiate(itemPrefab, this.listMain.transform);
                UIRideItem ui = go.GetComponent<UIRideItem>();
                ui.SetRideItem(kv.Value, this);
                this.listMain.AddItem(ui);
            }

        }
    }

    public void DoRide()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        if (this.selectedItem == null)
        {
            MessageBox.Show("请选择要转换的坐骑", "提示");
            return;
        }
        User.Instance.Ride(this.selectedItem.item.Id);
    }
}
