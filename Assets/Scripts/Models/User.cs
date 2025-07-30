using Common.Data;
using SkillBridge.Message;
using UnityEngine;

namespace Models
{
	class User : Singleton<User>
	{
		SkillBridge.Message.NUserInfo userInfo;


		public SkillBridge.Message.NUserInfo Info
		{
			get { return userInfo; }
		}


		public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
		{
			this.userInfo = info;
		}


		public MapDefine CurrentMapData { get; set; }

		public SkillBridge.Message.NCharacterInfo CurrentCharacter { get; set; }

		public PlayerInputController CurrentCharacterObject { get; set; }

		public NTeamInfo TeamInfo { get; set; }

		public void AddGold(int gold)
		{
			this.CurrentCharacter.Gold += gold;
		}
		public int CurrentRide = 0;
		public void Ride(int id)
		{
			if (CurrentRide != id)
			{
				CurrentRide = id;
				CurrentCharacterObject.SendEntityEvent(EntityEvent.Ride, CurrentRide);
			}
			else
			{
				CurrentRide = 0;
				CurrentCharacterObject.SendEntityEvent(EntityEvent.Ride, 0);
			}
		}
		//TODO: 初始化方法()  当前角色身上属性 
		void InitAttributes()
		{



		}
	}
}
