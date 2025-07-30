using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISystemConfig : UIWindow {
  
    public Toggle toggleMusic;
    public Toggle toggleSound;

    public Slider sliderMusic;
    public Slider sliderSound;

	void Start () {
        this.toggleMusic.isOn = Config.MusicOn;
        this.toggleSound.isOn = Config.SoundOn;
        this.sliderMusic.value = Config.MusicVoluem;
        this.sliderSound.value = Config.SoundVolume;
	}
    public override void OnYesClick()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        PlayerPrefs.Save();
        base.OnYesClick();
    }
    public void MusicToogle()
    {
        Config.MusicOn = this.toggleMusic.isOn;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }
    public void SoundToogle()
    {
        Config.SoundOn = this.toggleSound.isOn;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }
    public void MusicVolume(float value)
    {
        Config.MusicVoluem = (int)value;
    }
    public void SoundVolume(float value)
    {
        Config.SoundVolume = (int)value;
        PlaySound();
    }

    float lastPlay = 0;
    private void PlaySound()
    {
        if (Time.realtimeSinceStartup - lastPlay > 0.1)
        {
            lastPlay = Time.realtimeSinceStartup;
            SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        }
    }
    
}
