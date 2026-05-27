using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleManager : MonoBehaviour
{
    [Header("拼图列表（这一组的）")]
    public List<PuzzlePiece> pieces = new List<PuzzlePiece>();

    [Header("完成事件")]
    public UnityEvent onPuzzleComplete;

    private int placedCount = 0;

    private HashSet<PuzzleTarget> completedTargets = new HashSet<PuzzleTarget>();

    void OnEnable()
    {
        placedCount = 0;
        completedTargets.Clear();
    }

    // 👉 拼图完成时调用
    public void NotifyPiecePlaced(PuzzlePiece piece)
    {
        if (piece == null || piece.target == null)
        {
            Debug.LogWarning("❌ 拼图或目标为空");
            return;
        }

        if (completedTargets.Contains(piece.target))
        {
            Debug.Log("⚠️ 这个目标已经算过了，不重复计数");
            return;
        }

        completedTargets.Add(piece.target);
        placedCount++;

        Debug.Log($"[{name}] 完成进度：{placedCount}/{pieces.Count}");

        if (placedCount >= pieces.Count)
        {
            Debug.Log($"🎉 拼图完成：{name}");
            onPuzzleComplete?.Invoke();
        }
    }
}