using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBox 
{
    static Object cacheObject = null;
    public static UIInputBox Show(string message,string title="",string btnOk="",string btnCancel="",string emptyTips = "")
    {
        if (cacheObject == null) { cacheObject = Resloader.Load<Object>("UI/UIInputBox"); }

        GameObject go = (GameObject)GameObject.Instantiate(cacheObject);
        UIInputBox box = go.GetComponent<UIInputBox>();
        box.Init(title, message, btnOk, btnCancel, emptyTips);
        return box;
    }
}
