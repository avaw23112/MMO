using Common.Data;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MapTools
{
    /// <summary>
    /// �������͵�
    /// </summary>
    [MenuItem("Map Tools/Export Teleporters")]
    public static void ExportTeleporters()
    {
        DataManager.Instance.Load(); //�����ݼ��ص��ڴ���

        Scene current = EditorSceneManager.GetActiveScene();//��ȡ��ǰ������ʲô

        string currentScene = current.name;//���������ּ�¼����
        //�жϵ�ǰ�����Ƿ񱣴�
        if (current.isDirty)
        {
            EditorUtility.DisplayDialog("��ʾ", "���ȱ��浱ǰ����", "ȷ��");
            return;
        }

        List<TeleporterObject> allteleporters = new List<TeleporterObject>();

        foreach (var map in DataManager.Instance.Maps)
        {
            //�������г�����ԭʼ·��
            string sceneFile = "Assets/Levels/" + map.Value.Resource + ".unity";
            if (!System.IO.File.Exists(sceneFile))
            {
                Debug.LogWarningFormat("Scene  {0} not existed!", sceneFile);
                continue;
            }
            //�򿪵�ǰ����
            EditorSceneManager.OpenScene(sceneFile, OpenSceneMode.Single);
            //�ҵ���ǰ���������еĴ��͵�
            TeleporterObject[] teleporters = GameObject.FindObjectsOfType<TeleporterObject>();
            //����ÿ�����͵�
            foreach (var teleporter in teleporters)
            {

                if (!DataManager.Instance.Teleporters.ContainsKey(teleporter.ID))
                {
                    EditorUtility.DisplayDialog("����", string.Format("��ͼ��{0} �����õ� Teleporter;[{1}] �в�����", map.Value.Resource, teleporter.ID), "ȷ��");
                    return;
                }
                TeleporterDefine def = DataManager.Instance.Teleporters[teleporter.ID];
                if (def.MapID != map.Value.ID)
                {

                    EditorUtility.DisplayDialog("����", string.Format("��ͼ��{0} �����õ� Teleporter;[{1}] MapID: {2} ����", map.Value.Resource, teleporter.ID, def.MapID), "ȷ��");
                    return;
                }
                def.Position = GameObjectTool.WorldToLogicN(teleporter.transform.position);
                def.Direction = GameObjectTool.WorldToLogicN(teleporter.transform.forward);

            }

        }
        DataManager.Instance.SaveTeleporters();
        EditorSceneManager.OpenScene("Assets/Levels/" + currentScene + ".unity");
        EditorUtility.DisplayDialog("��ʾ", "���͵㵼�����", "ȷ��");//��ʾ�Ի���

    }

    [MenuItem("Map Tools/Export SpawnPoints")]

    public static void ExportSpawnPoints()
    {
        DataManager.Instance.Load(); //��ԭ�������ñ����ݼ���һ��

        Scene current = EditorSceneManager.GetActiveScene(); //��ȡ��ǰ����
        string currentScene = current.name;

        if (current.isDirty)
        {
            EditorUtility.DisplayDialog("��ʾ", "���ȱ��浱ǰ����", "ȷ��");
            return;

        }
        if (DataManager.Instance.SpawnPoints == null)
            DataManager.Instance.SpawnPoints = new Dictionary<int, Dictionary<int, SpawnPointDefine>>();

        foreach (var map in DataManager.Instance.Maps) //����ÿһ�ŵ�ͼ
        {
            string sceneFile = "Assets/Levels/" + map.Value.Resource + ".unity";
            if (!System.IO.File.Exists(sceneFile))  // �жϸ�·����û�и��ļ�
            {
                Debug.LogWarningFormat("Scene{0} not existed!", sceneFile);
                continue;
            }
            EditorSceneManager.OpenScene(sceneFile, OpenSceneMode.Single);//�򿪳���

            SpawnPoint[] spawnpoints = GameObject.FindObjectsOfType<SpawnPoint>();

            if (!DataManager.Instance.SpawnPoints.ContainsKey(map.Value.ID))
            {
                DataManager.Instance.SpawnPoints[map.Value.ID] = new Dictionary<int, SpawnPointDefine>();
            }
            foreach (var sp in spawnpoints)
            {
                if (!DataManager.Instance.SpawnPoints[map.Value.ID].ContainsKey(sp.ID))
                {
                    DataManager.Instance.SpawnPoints[map.Value.ID][sp.ID] = new SpawnPointDefine();
                }

                SpawnPointDefine def = DataManager.Instance.SpawnPoints[map.Value.ID][sp.ID];
                def.ID = sp.ID;
                def.MapID = map.Value.ID;
                def.Position = GameObjectTool.WorldToLogicN(sp.transform.position);
                def.Direction = GameObjectTool.WorldToLogicN(sp.transform.forward);
            }
        }
        DataManager.Instance.SaveSpawnPoints();
        EditorSceneManager.OpenScene("Assets/Levels/" + currentScene + ".unity");
        EditorUtility.DisplayDialog("��ʾ", "ˢ�ֵ㵼�����", "ȷ��");
    }

    [MenuItem("Map Tools/Generte NavData")]

    public static void GenerateNavData()
    {
        Material red = new Material(Shader.Find("Standard"));
        red.color = Color.red;
        red.SetColor("_TintColor", Color.red);
        red.enableInstancing = true;
        GameObject go = GameObject.Find("MinimapBoudingBox");
        if (go != null)
        {
            GameObject root = new GameObject("Root");
            BoxCollider bound = go.GetComponent<BoxCollider>();
            float step = 1f;
            for (float x = bound.bounds.min.x; x < bound.bounds.max.x; x += step)
            {
                for (float z = bound.bounds.min.z; z < bound.bounds.max.z; z += step)
                {
                    for (float y = bound.bounds.min.y; y < bound.bounds.max.y + 5f; y += step)
                    {
                        var pos = new Vector3(x, y, z);
                        NavMeshHit hit;
                        if (NavMesh.SamplePosition(pos, out hit, 0.5f, NavMesh.AllAreas))
                        {
                            if (hit.hit)
                            {
                                var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                box.name = "Hit" + hit.mask;
                                box.GetComponent<MeshRenderer>().sharedMaterial = red;
                                box.transform.SetParent(root.transform, true);
                                box.transform.position = pos;
                                box.transform.localScale = Vector3.one * 0.9f;
                            }
                        }
                    }
                }
            }
        }
    }
}
