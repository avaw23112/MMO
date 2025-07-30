using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuestDialog : UIWindow
{
    public Quest quest;
    public GameObject openButtons;
    public GameObject submitButton;
    public UIQuestInfo questInfo;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetQuest(Quest quest)
    {
        this.quest = quest;
        this.UpdateQuesst();
        if (this.quest.Info ==null)
        {
            openButtons.SetActive(true);
            submitButton.SetActive(false);
        }
        else
        {
            if (this.quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
            {
                openButtons.SetActive(false);
                submitButton.SetActive(true);
            }
            else
            {
                openButtons.SetActive(false);
                submitButton.SetActive(false);
            }
        }
    }

    void UpdateQuesst()
    {
        if(this.quest!=null)
        {
            if (this.questInfo !=null)
            {
                this.questInfo.SetQuestInfo(quest);
            }
        }
    }
}
