using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class FindComponentOnAllObject : EditorWindow
{
    [MenuItem("Tools/查找启用某组件的物体")]
    public static void ShowWindow()
    {
        GetWindow<FindComponentOnAllObject>("组件查找器");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("查找启用OcclusionArea的物体"))
        {
            FindEnabledComponents<OcclusionArea>();
        }
    }

    private static void FindEnabledComponents<T>() where T : Component
    {
        List<GameObject> results = new List<GameObject>();
        // 获取场景中所有游戏对象（包括未激活的）
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);

        foreach (GameObject obj in allObjects)
        {
            T component = obj.GetComponent<T>();
            if (component != null)
            {
                results.Add(obj);
                Debug.Log($"找到启用组件: {obj.name}", obj);
            }
        }

        Debug.Log($"已找到 {results.Count} 个物体启用了 {typeof(T).Name}");

        // 在Hierarchy中选中结果
        if (results.Count > 0)
        {
            Selection.objects = results.ToArray();
        }
    }
}