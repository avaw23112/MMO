using Managers;
using Services;
using SkillBridge.Message;
using UnityEngine;

class UIGuildApplyList : UIWindow
{
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;

    private void Start()
    {
        GuildService.Instance.OnGuildUpdate += UpdateList;
        //发送这个的目的只是为了触发公会系统的后处理，让最新的申请列表发过来。刷新公会列表是顺带的。
        GuildService.Instance.SendGuildListRequest();
        this.UpdateList();
    }
    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateList;
    }
    private void UpdateList()
    {
        ClearList();
        InitItems();
    }

    private void InitItems()
    {
        foreach (var item in GuildManager.Instance.guildInfo.Applies)
        {
            if (item.Result == ApplyResult.None)
            {
                UIGuildApplyItem ui = listMain.InstantiateItem<UIGuildApplyItem>(itemPrefab, itemRoot);
                ui.SetItemInfo(item);
                ui.closeAction = () => { this.OnCloseClisck(); };
                this.listMain.AddItem(ui);
            }
        }
    }

    private void ClearList()
    {
        this.listMain.RemoveAll();
    }

}


