using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;


namespace Assets.Scripts.Managers
{
    class ChatManager : Singleton<ChatManager>
    {
        public enum LocalChannel
        {
            All = 0,//所有
            Local = 1,//本地
            World = 2,//世界
            Team = 3,//队伍
            Guild = 4,//公会
            Private = 5,//私聊
        }

        private ChatChannel[] ChannelFilter = new ChatChannel[6]
        {
            ChatChannel.Local | ChatChannel.World | ChatChannel.Team | ChatChannel.Private | ChatChannel.System,
            ChatChannel.Local,
            ChatChannel.World,
            ChatChannel.Team,
            ChatChannel.Guild,
            ChatChannel.Private,
        };
        /// <summary>
        /// 发起私聊
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="targetName"></param>
        internal void StartPrivateChat(int targetId, string targetName)
        {
            this.PrivateID = targetId;
            this.PrivateName = targetName;
            this.sendChannel = LocalChannel.Private;
            if (this.OnChat != null)
                this.OnChat();
        }
        public List<ChatMessage>[] Messages = new List<ChatMessage>[6]
        {
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
        };

        public LocalChannel displayChannel;
        public LocalChannel sendChannel;
        public int PrivateID = 0;
        public string PrivateName = "";
        public ChatChannel SendChannel
        {
            get
            {
                switch (sendChannel)
                {
                    case LocalChannel.Local: return ChatChannel.Local;
                    case LocalChannel.World: return ChatChannel.World;
                    case LocalChannel.Team: return ChatChannel.Team;
                    case LocalChannel.Guild: return ChatChannel.Guild;
                    case LocalChannel.Private: return ChatChannel.Private;
                }
                return ChatChannel.Local;
            }
        }

        public Action OnChat { get; internal set; }

        public void Init()
        {
            foreach (var messages in this.Messages)
            {
                messages.Clear();
            }
        }

        public void SendChat(string content, int toId = 0, string toName = "")
        {
            ChatService.Instance.SendChat(this.SendChannel, content, toId, toName);
        }

        public bool SetSendChannel(LocalChannel channel)
        {
            if (channel == LocalChannel.Team)
            {
                if (User.Instance.TeamInfo == null)
                {
                    this.AddSystemMessage("你没有加入任何队伍");
                    return false;
                }
            }
            if (channel == LocalChannel.Guild)
            {
                if (User.Instance.CurrentCharacter.Guild == null)
                {
                    this.AddSystemMessage("你没有加入任何公会");
                    return false;
                }
            }
            this.sendChannel = channel;
            UnityEngine.Debug.LogFormat("Set Channel:{0}", this.sendChannel);
            return true;
        }

        public void AddMessages(ChatChannel channel, List<ChatMessage> messages)
        {
            for (int ch = 0; ch < 6; ch++)
            {
                if ((this.ChannelFilter[ch] & channel) == channel)
                {
                    this.Messages[ch].AddRange(messages);
                }
            }
            if (this.OnChat != null)
            {
                this.OnChat();
            }
        }

        public void AddSystemMessage(string message, string form = "")
        {
            this.Messages[(int)LocalChannel.All].Add(new ChatMessage()
            {
                Channel = ChatChannel.System,
                Message = message,
                FromName = form
            });
            if (this.OnChat != null)
                this.OnChat();
        }
        StringBuilder sb = new StringBuilder();
        public StringBuilder GetCurrentMessages()
        {
            sb.Clear();
            foreach (var message in this.Messages[(int)displayChannel])
            {
                FormatMessageBySB(sb, message);
                sb.AppendLine();
            }
            return sb;
        }
        string FormatMessage(ChatMessage message)
        {
            switch (message.Channel)
            {
                case ChatChannel.Local:
                    return string.Format("[本地]{0}{1}", FormatFromPlayer(message), message.Message);
                case ChatChannel.World:
                    return string.Format("<color=purple>[世界]{0}{1}</color>", FormatFromPlayer(message), message.Message);

                case ChatChannel.System:
                    return string.Format("<color=yellow>[系统]{0}</color>", message.Message);

                case ChatChannel.Private:
                    return string.Format("<color=red>[私聊]{0}{1}</color>", FormatFromPlayer(message), message.Message);

                case ChatChannel.Team:
                    return string.Format("<color=green>[队伍]{0}{1}</color>", FormatFromPlayer(message), message.Message);

                case ChatChannel.Guild:
                    return string.Format("<color=blue>[公会]{0}{1}</color>", FormatFromPlayer(message), message.Message);
                default:
                    break;
            }
            return "";
        }
        string FormatFromPlayer(ChatMessage message)
        {
            if (message.FromId == User.Instance.CurrentCharacter.Id)
            {
                //return "<a name=\"\"class=\"player\">[我]</a>";
                return "<link=\"\"><#00FFE0><u>[我]</u></color></link>";
            }
            else
                return string.Format("<link=\"{0}:{1}\"><#00FFE0><u>[{1}]</u></color></link>", message.FromId, message.FromName);
            //return string.Format("<a name=\"c:{0}:{1}\" class=\"player\">[{1}]</a>", message.FromId,message.FromName);
        }
        StringBuilder FormatMessageBySB(StringBuilder sb, ChatMessage message)
        {
            switch (message.Channel)
            {
                case ChatChannel.Local:
                    sb.Append("[本地]");
                    AppendFormattedPlayer(sb, message);
                    sb.Append(message.Message);
                    break;
                case ChatChannel.World:
                    sb.Append("<color=purple>[世界]");
                    AppendFormattedPlayer(sb, message);
                    sb.Append(message.Message);
                    sb.Append("</color>");
                    break;
                case ChatChannel.System:
                    sb.Append("<color=yellow>[系统]");
                    sb.Append(message.Message);
                    sb.Append("</color>");
                    break;
                case ChatChannel.Private:
                    sb.Append("<color=red>[私聊]");
                    AppendFormattedPlayer(sb, message);
                    sb.Append(message.Message);
                    sb.Append("</color>");
                    break;
                case ChatChannel.Team:
                    sb.Append("<color=green>[队伍]");
                    AppendFormattedPlayer(sb, message);
                    sb.Append(message.Message);
                    sb.Append("</color>");
                    break;
                case ChatChannel.Guild:
                    sb.Append("<color=blue>[公会]");
                    AppendFormattedPlayer(sb, message);
                    sb.Append(message.Message);
                    sb.Append("</color>");
                    break;
            }
            return sb;
        }

        void AppendFormattedPlayer(StringBuilder sb, ChatMessage message)
        {
            if (message.FromId == User.Instance.CurrentCharacter.Id)
            {
                sb.Append("<link=\"\"><#00FFE0><u>[我]</u></color></link>");
            }
            else
            {
                sb.Append("<link=\"");
                sb.Append(message.FromId);
                sb.Append(":");
                sb.Append(message.FromName);
                sb.Append("\"><#00FFE0><u>[");
                sb.Append(message.FromName);
                sb.Append("]</u></color></link>");
            }
        }
    }
}
