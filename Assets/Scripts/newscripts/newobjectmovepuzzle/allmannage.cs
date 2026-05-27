using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AllPuzzleManager : MonoBehaviour
{
    [Header("需要全部完成的拼图组")]
    public List<PuzzleManager> managers = new List<PuzzleManager>();

    [Header("目标按钮")]
    public Button targetButton;

    [Header("全部完成事件")]
    public UnityEvent onAllComplete;

    private int completeCount = 0;
    private bool isFinished = false;

    void Start()
    {
        if (targetButton != null)
            targetButton.interactable = false;

        // 绑定每个 PuzzleManager 的完成事件
        foreach (var m in managers)
        {
            if (m != null)
                m.onPuzzleComplete.AddListener(OnOneGroupComplete);
        }
    }

    void OnOneGroupComplete()
    {
        if (isFinished) return;

        completeCount++;

        Debug.Log($"🧩 拼图组完成进度：{completeCount}/{managers.Count}");

        if (completeCount >= managers.Count)
        {
            isFinished = true;

            Debug.Log("🎉 所有拼图完成！");

            if (targetButton != null)
                targetButton.interactable = true;

            onAllComplete?.Invoke();
        }
    }
}