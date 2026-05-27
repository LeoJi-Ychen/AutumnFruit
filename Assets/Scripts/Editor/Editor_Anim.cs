using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Effect_Anim))]
public class Effect_AnimEditor : Editor
{
    SerializedProperty routesProp;
    List<DefaultAsset> tempFolders = new List<DefaultAsset>();

    void OnEnable()
    {
        routesProp = serializedObject.FindProperty("routes");

        // 尝试从现有路径恢复文件夹引用（可选）
        tempFolders.Clear();
        for (int i = 0; i < routesProp.arraySize; i++)
        {
            string path = routesProp.GetArrayElementAtIndex(i).stringValue;
            Object folder = AssetDatabase.LoadAssetAtPath<DefaultAsset>(path);
            tempFolders.Add(folder as DefaultAsset);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Effect_Anim script = (Effect_Anim)target;

        // 绘制默认属性
        EditorGUILayout.PropertyField(serializedObject.FindProperty("frameTime"));     
        EditorGUILayout.PropertyField(serializedObject.FindProperty("playMode"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("animObject"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("routes"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("anim_index"));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("动画文件夹 (Resources 下)", EditorStyles.boldLabel);

        // 动态调整列表大小
        int newCount = Mathf.Max(1,EditorGUILayout.IntField("文件夹数量", tempFolders.Count));
        while (tempFolders.Count < newCount) tempFolders.Add(null);
        while (tempFolders.Count > newCount) tempFolders.RemoveAt(tempFolders.Count - 1);

        // 绘制文件夹选择
        for (int i = 0; i < tempFolders.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            tempFolders[i] = EditorGUILayout.ObjectField(
                $"文件夹 {i}",
                tempFolders[i],
                typeof(DefaultAsset),
                false
            ) as DefaultAsset;

            if (GUILayout.Button("移除", GUILayout.Width(50)))
            {
                tempFolders.RemoveAt(i);
                break;
            }

            EditorGUILayout.EndHorizontal();

            // 显示路径预览
            if (tempFolders[i] != null)
            {
                string path = AssetDatabase.GetAssetPath(tempFolders[i]);
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField($"路径: {path}", EditorStyles.miniLabel);
                EditorGUI.indentLevel--;
            }
        }

        // 应用路径到运行时列表
        if (GUILayout.Button("应用路径", GUILayout.Height(30)))
        {
            routesProp.ClearArray();
            foreach (var folder in tempFolders)
            {
                if (folder != null)
                {
                    string path = AssetDatabase.GetAssetPath(folder);
                    string route = ConvertToResourcesPath(path);
                    routesProp.InsertArrayElementAtIndex(routesProp.arraySize);
                    routesProp.GetArrayElementAtIndex(routesProp.arraySize - 1).stringValue = route;
                }
            }
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
            Debug.Log("路径已应用！");
        }

        // 验证警告
        bool hasInvalidPath = false;
        foreach (var folder in tempFolders)
        {
            if (folder != null)
            {
                string path = AssetDatabase.GetAssetPath(folder);
                if (!path.Contains("/Resources/"))
                {
                    hasInvalidPath = true;
                    break;
                }
            }
        }

        if (hasInvalidPath)
        {
            EditorGUILayout.HelpBox("警告：文件夹必须在 Resources 目录下才能运行时加载！", MessageType.Warning);
        }

        serializedObject.ApplyModifiedProperties();
    }
    string ConvertToResourcesPath(string assetPath)
    {
        if (assetPath.Contains("Resources/"))
        {
            int index = assetPath.IndexOf("Resources/") + "Resources/".Length;
            return assetPath.Substring(index);
        }
        return assetPath;
    }
}