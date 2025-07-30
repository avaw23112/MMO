using Managers;
using Models;
using Services;
using SkillBridge.Message;
using UnityEngine;

public class UIGuild : UIWindow {
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIGuildInfo uiInfo;
    public UIGuildMemberItem selectedItem;
    public GameObject panelAdmin;
    public GameObject panelLeader;

	void Start () {
        GuildService.Instance.OnGuildUpdate += UpdateUI;
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
        this.UpdateUI();

    }
  
    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateUI;
    }
    private void UpdateUI()
    {
        this.uiInfo.Info = GuildManager.Instance.guildInfo;

        ClearList();
        InitItems();

        this.panelAdmin.SetActive(GuildManager.Instance.myMemberInfo.Title > GuildTitle.None);
        this.panelLeader.SetActive(GuildManager.Instance.myMemberInfo.Title == GuildTitle.President);
    }

    public void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIGuildMemberItem;
    }
    /// <summary>
    /// 初始化所有装备列表
    /// </summary>
    void InitItems()
    {
        foreach (var item in GuildManager.Instance.guildInfo.Members)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIGuildMemberItem ui = go.GetComponent<UIGuildMemberItem>();
            ui.SetGuildMemberInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    void ClearList()
    {
        this.listMain.RemoveAll();
    }
    public void OnClickAppliesList()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        UIManager.Instance.Show<UIGuildApplyList>();
    }
	//扩展作业 离开公会
    public void OnClickLeave()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        if (uiInfo.Info.leaderId == User.Instance.CurrentCharacter.Id)
        {
            MessageBox.Show("你是会长不能离开公会哦!", "离开工会", MessageBoxType.Confirm);
            return;
        }
        MessageBox.Show("确定要离开工会吗？", "离开工会", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendGuildLeaveRequest();
        };
    }
    public void OnClickChat()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要私聊的成员");
            return;
        }
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        //ChatManager.Instance.StartPrivateChat(selectedItem.Info.Info.Id, selectedItem.Info.Info.Name);
    }
    public void OnClickKickout()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要踢出的成员");
            return;
        }
        MessageBox.Show(string.Format("要踢【{0}】出公会吗？", this.selectedItem.Info.Info.Name), "踢出公会",MessageBoxType.Confirm).OnYes = () =>
         {
             GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout, this.selectedItem.Info.Info.Id);
         };
    }
    public void OnClickPromote()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        if (selectedItem == null)
        {
            MessageBox.Show("选择要晋升的成员");
        }
        if (selectedItem.Info.Title != GuildTitle.None)
        {
            MessageBox.Show("对方已经有身份尊贵");
        }
        MessageBox.Show(string.Format("要晋升【{0}】为公会副会长吗？", this.selectedItem.Info.Info.Name), "晋升", MessageBoxType.Confirm).OnYes = () =>
           {
               GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote, this.selectedItem.Info.Info.Id);
           };
    }
    public void OnClickDepose()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);

        if (selectedItem == null)
        {
            MessageBox.Show("请选择要罢免的成员");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.None)
        {
            MessageBox.Show("对方貌似无职可免");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.President)
        {
            MessageBox.Show("会长岂是尔等宵小能撼动的");
            return;
        }
        selectedItem.Info.Title = GuildTitle.President;
        MessageBox.Show(string.Format("要罢免【0】的工会职务吗？", this.selectedItem.Info.Info.Name), "罢免会长", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depost, this.selectedItem.Info.Info.Id);
        };
    }
    public void OnClickTransfer()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);

        if (selectedItem == null)
        {
            MessageBox.Show("请选择要把会长转让给的成员");
            return;
        }
        if (selectedItem.Info.Info.Id == User.Instance.CurrentCharacter.Id)
        {
            MessageBox.Show("你已经是会长了");
            return;
        }
        MessageBox.Show(string.Format("要把会长转让给【0】吗？", this.selectedItem.Info.Info.Name), "转移会长", MessageBoxType.Confirm).OnYes = () =>
           {
               GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer, this.selectedItem.Info.Info.Id);
           };
    }
    //扩展作业修改工会宣言
    public void OnClickSetNotice()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);

        InputBox.Show("请输入公会宣言", "修改公会宣言", "确定", "取消", "UIInputBoxEnum.big").OnSubmit += OnSetNotice;
    }
    public bool OnSetNotice(string input, out string tips)
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        tips = "";
        if (4 < input.Length && input.Length < 50)
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Notice, uiInfo.Info.leaderId, input);
            return true;
        }
        else
        {
            tips = "公会宣言必须再4字以上50字以下哦";
            return false;
        }
    }

}
