using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Managers;
using Managers;
using Models;
using Network;
using SkillBridge.Message;
using UnityEngine;

namespace Services
{
    public class ChatService : Singleton<ChatService>
    {
        public ChatService()
        {
            MessageDistributer.Instance.Subscribe<ChatResponse>(this.OnChatResponse);

        }
        
        public void Init()
        {

        }
        public void SendChat(ChatChannel sendChannel, string content, int toId, string toName)
        {
            Debug.Log("SendChat");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.Chat = new ChatRequest();
            message.Request.Chat.Message = new ChatMessage();
            message.Request.Chat.Message.Channel = sendChannel;
            message.Request.Chat.Message.FromId = User.Instance.CurrentCharacter.Id;
            message.Request.Chat.Message.FromName = User.Instance.CurrentCharacter.Name;
            message.Request.Chat.Message.ToId = toId;
            message.Request.Chat.Message.ToName = toName;
            message.Request.Chat.Message.Message = content;
            NetClient.Instance.SendMessage(message);
        }
        private void OnChatResponse(object sender, ChatResponse message)
        {
            if (message.guildMessages != null)
            {
                ChatManager.Instance.AddMessages(ChatChannel.Guild, message.guildMessages);
            }
            if (message.localMessages != null)
            {
                ChatManager.Instance.AddMessages(ChatChannel.Local, message.localMessages);
            }
            if (message.privateMessages != null)
            {
                ChatManager.Instance.AddMessages(ChatChannel.Private, message.privateMessages);
            }
            if (message.teamMessages != null)
            {
                ChatManager.Instance.AddMessages(ChatChannel.Team, message.teamMessages);
            }
            if (message.worldMessages != null)
            {
                ChatManager.Instance.AddMessages(ChatChannel.World, message.worldMessages);
            }
            if (message.systemMessages != null)
            {
                ChatManager.Instance.AddMessages(ChatChannel.System, message.systemMessages);
            }
        }

    }
}
