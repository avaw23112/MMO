using Assets.Scripts.Managers;
using Assets.Scripts.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopCharMenu : UIWindow,IDeselectHandler
{
    public int targetId;
    public string targetName;

    // 取消选择
    public void OnDeselect(BaseEventData eventData)
    {
        var ed = eventData as PointerEventData; 
        if (ed.hovered.Contains(this.gameObject))  
            return;
        this.Close(WindowResult.None);
    }
    public void OnEnable()
    {
        this.GetComponent<Selectable>().Select();  // 选择状态
        //this.Root.transform.position = Input.mousePosition + new Vector3(80,0,0);
       UpdateRootPos();
    }

    public void UpdateRootPos()
    {
        RectTransform rectTransform = this.Root.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = (Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f, 0)) * 0.5f + new Vector3(80f, 0, 0);
        
    }
    public void OnChat()
    {
        ChatManager.Instance.StartPrivateChat(targetId,targetName);
        this.Close(WindowResult.No);
    }
    //拓展作业 聊天添加好友
    public void OnAddFirend()
    {
        FriendService.Instance.SendFriendAddRequest(targetId, targetName);
        this.Close(WindowResult.No);
    }
    //拓展作业 聊天添加小队
    public void OnInviteTeam()
    {
        TeamService.Instance.SendTeamInviteRequest(targetId, targetName);
        this.Close(WindowResult.No);
    }
}
