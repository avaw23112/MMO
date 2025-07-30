using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildPopCreate : UIWindow
{

    public InputField inputName;
    public InputField inputNotice;


    void Start()
    {
        GuildService.Instance.OnGuildCreateResult = OnGuildCreated;
    }
    private void OnDestroy()
    {
        GuildService.Instance.OnGuildCreateResult = null;
    }
    public override void OnYesClick()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);

        if (string.IsNullOrEmpty(inputName.text))
        {
            MessageBox.Show("请输入工会的名称", "错误", MessageBoxType.Error);
            return;
        }
        if (inputName.text.Length < 4 || inputName.text.Length > 10)
        {
            MessageBox.Show("工会名称为4—10字符", "错误", MessageBoxType.Error);
            return;
        }
        if (string.IsNullOrEmpty(inputNotice.text))
        {
            MessageBox.Show("请输入工会宣言", "错误", MessageBoxType.Error);
            return;
        }
        if (inputNotice.text.Length < 3 || inputNotice.text.Length > 50)
        {
            MessageBox.Show("工会宣言需为4—10字符", "错误", MessageBoxType.Error);
            return;
        }
        GuildService.Instance.SendGuildCreate(inputName.text, inputNotice.text);
    }
    void OnGuildCreated(bool result)
    {

        if (result)
        {
            this.Close(WindowResult.Yes);
        }
    }

}
