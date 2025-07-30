using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharInfo : MonoBehaviour {


    public SkillBridge.Message.NCharacterInfo info;

    public Text charClass;
    public Text charName;
    public Image highlight;
    public Button deleteBtn;

    public bool Selected
    {
        get { return highlight.IsActive(); }
        set
        {
            highlight.gameObject.SetActive(value);
        }
    }

    // Use this for initialization
    void Start () {
		if(info!=null)
        {
            this.charClass.text = this.info.Class.ToString();
            this.charName.text = this.info.Name;
            this.deleteBtn.onClick.AddListener(OnDeleteClick);
        }
	}
    // 点击了删除角色按钮
    void OnDeleteClick() 
    {
        UserService.Instance.SendCharacterDelete(info);
    }
}
