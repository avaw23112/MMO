using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;
using System;

public class UILogin : MonoBehaviour {

    public InputField username;
    public InputField password;
    public Toggle isremember;
    public Button buttonLogin;
    public Button buttonRegister;

    // Use this for initialization
    void Start () {
        this.InitUI();
        UserService.Instance.OnLogin = OnLogin;

        //isremember.onValueChanged.AddListener(OnToggleIsOn);
    }

    void InitUI() {
        if (PlayerPrefs.GetInt("isremember") != 0)
        {
            this.username.text = PlayerPrefs.GetString("username");
            this.password.text = PlayerPrefs.GetString("password");
            this.isremember.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("isremember"));
        }
        else {
            this.username.text = "";
            this.password.text = "";
            this.isremember.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("isremember"));
        }
    }

    public void OnToggleIsOn(bool isOn) {
        if (isOn)
        {
            PlayerPrefs.SetString("username", this.username.text);
            PlayerPrefs.SetString("password", this.password.text);
        }
        else
        {
            PlayerPrefs.SetString("username", string.Empty);
            PlayerPrefs.SetString("password", string.Empty);
        }
        PlayerPrefs.SetInt("isremember", Convert.ToInt32(isOn));
    }

    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(this.username.text))
        {
            MessageBox.Show("请输入账号");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }
        this.OnToggleIsOn(this.isremember.isOn);
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        // Enter Game
        UserService.Instance.SendLogin(this.username.text,this.password.text);

    }

    void OnLogin(Result result, string message)
    {
        if (result == Result.Success)
        {
            //登录成功，进入角色选择
            //MessageBox.Show("登录成功,准备角色选择" + message,"提示", MessageBoxType.Information);
            SceneManager.Instance.LoadScene("CharSelect");
            SoundManager.Instance.PlaySound(SoundDefine.Music_Select);

        }
        else
            MessageBox.Show(message, "错误", MessageBoxType.Error);
    }
}
