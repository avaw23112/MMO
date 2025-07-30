using Managers;
using Models;
using UnityEngine;
using UnityEngine.UI;


public class UIMain : MonoSingleton<UIMain>
{

    public Text avatarName;
    public Text avatarLevel;
    public UITeam TeamWindown;

    // Use this for initialization
    protected override void OnStart()
    {
        this.UpdateAvatar();

    }

    void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
        this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    UIPopCharMenu menu = UIManager.Instance.Show<UIPopCharMenu>();
        //}
    }

    public void OnClickTest()
    {
        var test = UIManager.Instance.Show<UITest>();
        test.OnClose += Test_OnClose;
    }

    private void Test_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        MessageBox.Show("点击了对话框的：" + result, "对话框响应结果", MessageBoxType.Information);
    }

    public void OnClickBag()
    {
        UIManager.Instance.Show<UIBag>();
    }

    public void OnClickCharEquip()
    {
        UIManager.Instance.Show<UICharEquip>();
    }

    public void OnClickQuest()
    {
        UIManager.Instance.Show<UIQuestSystem>();
    }
    public void OnClickFriend()
    {
        UIManager.Instance.Show<UIFriends>();
    }

    public void ShowTeamUI(bool show)
    {
        TeamWindown.ShowTeam(show);
    }

    public void OnClickGuild()
    {
        GuildManager.Instance.ShowGuild();
    }
    public void OnClickRide()
    {
        UIManager.Instance.Show<UIRide>();
    }
    public void OnClickSetting()
    {
        UIManager.Instance.Show<UISetting>();
    }
    //public void OnClickSkill()
    //{
    //    UIManager.Instance.Show<UISkill>();
    //}
}
