using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    public Sprite activeImage;
    public Sprite normalImage;

    public TabView tabView;

    public int tabIndex = 0;
    public bool selected = false;
    private Image tabImage;
    // Start is called before the first frame update
    void Start()
    {
        tabImage = GetComponent<Image>();
        normalImage = tabImage.sprite;

        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }
    public void Select(bool select)
    {
        tabImage.overrideSprite = select ? activeImage : normalImage;
    }
    private void OnClick()
    {
        this.tabView.SelectTab(this.tabIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
