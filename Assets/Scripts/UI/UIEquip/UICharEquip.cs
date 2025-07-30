using Assets.Scripts.Managers;
using Managers;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UICharEquip : UIWindow
{
    public Text Title;
    public Text Money;
    public GameObject ItemPrefab;
    public GameObject ItemEquipedPrefab;
    public Transform ItemListRoot;
    public Transform[] Slots;

    //public Text hp;
    //public Slider hpBar;

    //public Text mp;
    //public Slider mpBar;
    //public Text[] attrs;

    // Start is called before the first frame update
    void Start()
    {
        RefreshUI();
        EquipManager.Instance.OnEquipChanged += RefreshUI;
    }
    private void OnDestroy()
    {
        EquipManager.Instance.OnEquipChanged -= RefreshUI;

    }
    private void RefreshUI()
    {
        ClearAllEquipList();
        InitAllEquipItems();
        ClearEquipList();
        InitEquipedItems();
        this.Money.text = User.Instance.CurrentCharacter.Gold.ToString();

        //InitAttributes();
    }
    /// <summary>
    /// 初始化所有装备列表
    /// </summary>
    /// <exception></exception>
    private void InitAllEquipItems()
    {
        foreach (var kv in ItemManager.Instance.Items)
        {
            if (kv.Value.Define.Type == SkillBridge.Message.ItemType.Equip && kv.Value.Define.LimitClass == User.Instance.CurrentCharacter.Class)
            {
                //已经装备就不显示了
                if (EquipManager.Instance.Contains(kv.Key))
                    continue;
                GameObject go = Instantiate(ItemPrefab, ItemListRoot);
                UIEquipItem ui = go.GetComponent<UIEquipItem>();
                ui.SetEquipItem(kv.Key, kv.Value, this, false);
            }
        }
    }

    void ClearAllEquipList()
    {
        //for (int i = 0; i < ItemListRoot.transform.childCount; i++)
        //{
        //    Destroy(ItemListRoot.GetChild(i).gameObject);
        //}
        foreach (var item in ItemListRoot.GetComponentsInChildren<UIEquipItem>())
        {
            Destroy(item.gameObject);
        }
    }

    public void SetItemSelectOff()
    {
        foreach (var item in ItemListRoot.GetComponentsInChildren<UIEquipItem>())
        {
            item.Selected = false;
        }
    }

    void ClearEquipList()
    {
        foreach (var item in Slots)
        {
            if (item.childCount > 0)
                Destroy(item.GetChild(0).gameObject);
        }
    }
    /// <summary>
    /// 初始化已经装备的列表
    /// </summary>
    void InitEquipedItems()
    {
        for (int i = 0; i < (int)EquipSlot.SlotMax; i++)
        {
            var item = EquipManager.Instance.Equips[i];
            {
                if (item != null)
                {
                    GameObject go = Instantiate(ItemEquipedPrefab, Slots[i]);
                    UIEquipItem ui = go.GetComponent<UIEquipItem>();
                    ui.SetEquipItem(i, item, this, true);
                }
            }
        }
    }
    public void DoEquip(Item item)
    {
        EquipManager.Instance.EquipItem(item);
    }
    public void UnEquip(Item item)
    {
        EquipManager.Instance.UnEquipItem(item);
    }
    //public void Destroy()
    //{
    //    Destroy(this.gameObject);
    //}

    ///// <summary>
    ///// 角色属性初始化
    ///// </summary>
    //void InitAttributes()
    //{
    //    //TODO
    //    var character = User.Instance.CurrentCharacter.Attributes;
    //    this.hp.text = string.Format("{0}/{1}", character.HP, character.MaxHP);
    //    this.mp.text = string.Format("{0}/{1}", character.MP, character.MaxMP);
    //    this.hpBar.maxValue = character.MaxHP;
    //    this.hpBar.value = character.HP;

    //    this.mpBar.maxValue = character.MaxMP;
    //    this.mpBar.value = character.MP;

    //    for (int i = (int)AttributeType.STR; i < (int)AttributeType.MAX; i++)
    //    {
    //        //判断是否暴击 如果是暴击用百分比做初始化
    //        if (i == ((int)AttributeType.CRI))
    //            this.attrs[i - 2].text = string.Format("{0:f2}%", character.Final.Data[i] * 100);
    //        else //不是正常把数值转为string
    //            this.attrs[i-2].text = ((int)character.Final.Data[i]).ToString();
    //    }

    //}
        
}




