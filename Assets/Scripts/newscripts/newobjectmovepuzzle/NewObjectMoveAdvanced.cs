using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class NewObjectMoveAdvanced : MonoBehaviour
{
    public Transform target;
    public float duration = 0.8f;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(PlayRoutine());
    }

    IEnumerator PlayRoutine()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = target.position;

        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = t / duration;

            transform.position = Vector3.Lerp(startPos, endPos, p);
            canvasGroup.alpha = Mathf.Lerp(0, 1, p);

            yield return null;
        }
    }
}
