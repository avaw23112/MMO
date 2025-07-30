using Assets.Scripts.Managers;
using Candlelight.UI;
using Cysharp.Text;
using GameFramework;
using SkillBridge.Message;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChat : MonoBehaviour
{
    public TextMeshProUGUI textArea; // 聊天内容显示区域
    public TabView channelTab;
    public InputField chatText; // 聊天输入控件
    public Text chatTarge;
    public Dropdown channelSelect;

    // Start is called before the first frame update
    void Start()
    {

        channelTab.OnTabSelect += OnDisplayChannelSelected;
        ChatManager.Instance.OnChat += RefreshUI;
    }

    void OnDestroy()
    {
        ChatManager.Instance.OnChat -= RefreshUI;
    }
    // Update is called once per frame
    void Update()
    {
        InputManager.Instance.IsInputMode = chatText.isFocused;
    }
    void OnDisplayChannelSelected(int idx)
    {
        ChatManager.Instance.displayChannel = (ChatManager.LocalChannel)idx;
        RefreshUI();
    }
    void RefreshUI()
    {
        this.textArea.SetText(ChatManager.Instance.GetCurrentMessages());
        this.channelSelect.value = (int)ChatManager.Instance.sendChannel - 1;
        if (ChatManager.Instance.SendChannel == ChatChannel.Private)
        {
            this.chatTarge.gameObject.SetActive(true);
            if (ChatManager.Instance.PrivateID != 0)
            {
                this.chatTarge.text = ChatManager.Instance.PrivateName + ":";
            }
            else
                this.chatTarge.text = "<无>";
        }
        else
        {
            this.chatTarge.gameObject.SetActive(false);
        }
    }
    //因为HyperText已经停止更新，所以和新版本Unity，.Net有兼容问题
    //这里使用TMP实现超链接
    //相关逻辑转到 LinkOpener 脚本查看
    //public void onClickChatLink(HyperText text, HyperText.LinkInfo link)
    //{
    //    if (string.IsNullOrEmpty(link.Name)) return;
    //    //<a name ="c:1001:name" class="player">Name</a>
    //    //<a name ="i:1001:name" class="item">Name</a>
    //    if (link.Name.StartsWith("c:"))
    //    {
    //        string[] strs = link.Name.Split(":".ToCharArray());
    //        UIPopCharMenu menu = UIManager.Instance.Show<UIPopCharMenu>();
    //        menu.targetId = int.Parse(strs[1]);
    //        menu.targetName = strs[2];
    //    }
    //}
    public void onClickSend()
    {
        OnEndInput(this.chatText.text);
    }
    public void OnEndInput(string text)
    {
        if (!string.IsNullOrEmpty(text.Trim()))
        {
            this.SendChat(text);
        }
        this.chatText.text = "";
    }
    void SendChat(string text)
    {
        ChatManager.Instance.SendChat(text, ChatManager.Instance.PrivateID, ChatManager.Instance.PrivateName);
    }
    public void OnSendChannelChanged(int idx)
    {
        if (ChatManager.Instance.sendChannel == (ChatManager.LocalChannel)(idx + 1))
            return;
        if (!ChatManager.Instance.SetSendChannel((ChatManager.LocalChannel)idx + 1))
        {
            this.channelSelect.value = (int)ChatManager.Instance.sendChannel - 1;
        }
        else
        {
            this.RefreshUI();
        }
    }
}
