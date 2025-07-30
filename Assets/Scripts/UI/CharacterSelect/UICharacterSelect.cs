using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Services;
using SkillBridge.Message;


public class UICharacterSelect : MonoBehaviour
{

    public GameObject panelCreate;
    public GameObject panelSelect;

    public GameObject btnCreateCancel;

    public InputField charName;
    CharacterClass charClass;

    public Transform uiCharList;
    public GameObject uiCharInfo;

    public List<GameObject> uiChars = new List<GameObject>();

    public Image[] titles;

    public Text descs;


    public Text[] names;

    private int selectCharacterIdx = -1;

    public UICharacterView characterView;

    // Use this for initialization
    void Start()
    {
        // 这里判断如果当前用户没有角色的话 之间显示角色创建界面 否则显示角色列表
        if (User.Instance.Info.Player.Characters.Count > 0) 
            InitCharacterSelect(true);
        else
            InitCharacterCreate();

        UserService.Instance.OnCharacterCreate = OnCharacterCreate;
        UserService.Instance.OnCharacterDelete = OnCharacterDelete;
    }
    private void OnDestroy()
    {
        UserService.Instance.OnCharacterCreate -= OnCharacterCreate;
        UserService.Instance.OnCharacterDelete -= OnCharacterDelete;
    }


    public void InitCharacterSelect(bool init)
    {
        panelCreate.SetActive(false);
        panelSelect.SetActive(true);

        if (init)
        {
            foreach (var old in uiChars)
            {
                Destroy(old);
            }
            uiChars.Clear();

            if (User.Instance.Info.Player.Characters.Count <= 0)
            {
                //所有角色模型都隐藏
                OnSelectCharacter(-1);
                InitCharacterCreate();
                return;
            }

            for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
            {
                GameObject go = Instantiate(uiCharInfo, this.uiCharList);
                UICharInfo chrInfo = go.GetComponent<UICharInfo>();
                chrInfo.info = User.Instance.Info.Player.Characters[i];

                Button button = go.GetComponent<Button>();
                
                int idx = i;
                button.onClick.AddListener(() =>
                {
                    OnSelectCharacter(idx);
                });

                uiChars.Add(go);
                go.SetActive(true);
            }
        }
        if (User.Instance.Info.Player.Characters.Count <= 0)
            //所有角色模型都隐藏
            OnSelectCharacter(-1);
        else
            // 默认选择显示第一个角色
            OnSelectCharacter(0);
    }

    public void InitCharacterCreate()
    {
        panelCreate.SetActive(true);
        panelSelect.SetActive(false);
        OnSelectClass(1);
    }


    public void OnClickCreate()
    {
        if (string.IsNullOrEmpty(this.charName.text))
        {
            MessageBox.Show("请输入角色名称");
            return;
        }
        UserService.Instance.SendCharacterCreate(this.charName.text, this.charClass);
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }

    public void OnSelectClass(int charClass)
    {
        this.charClass = (CharacterClass)charClass;

        characterView.CurrectCharacter = charClass - 1;

        for (int i = 0; i < 3; i++)
        {
            titles[i].gameObject.SetActive(i == charClass - 1);
            names[i].text = DataManager.Instance.Characters[i + 1].Name;
        }

        descs.text = DataManager.Instance.Characters[charClass].Description;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }


    void OnCharacterCreate(Result result, string message)
    {
        if (result == Result.Success)
        {
            InitCharacterSelect(true);
        }
        else
            MessageBox.Show(message, "错误", MessageBoxType.Error);
    }

    private void OnCharacterDelete(Result result, string message)
    {
        if (result == Result.Success)
        {
            InitCharacterSelect(true);
            MessageBox.Show(message, "成功", MessageBoxType.Confirm);
        }
        else {
            MessageBox.Show(message, "错误", MessageBoxType.Error);
        }
            
    }


    public void OnSelectCharacter(int idx)
    {
        if (idx < 0) {
            this.selectCharacterIdx = -1;
            characterView.CurrectCharacter = -1;
            return;
        }
        this.selectCharacterIdx = idx;
        var cha = User.Instance.Info.Player.Characters[idx];
        Debug.LogFormat("Select Char:[{0}]{1}[{2}]", cha.Id, cha.Name, cha.Class);
        characterView.CurrectCharacter = ((int)cha.Class - 1);

        for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
        {
            UICharInfo ci = this.uiChars[i].GetComponent<UICharInfo>();
            ci.Selected = idx == i;
        }
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }
    public void OnClickPlay()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        if (selectCharacterIdx >= 0)
        {
            UserService.Instance.SendGameEnter(selectCharacterIdx);
        }
    }
}
