using UnityEngine;
using System.Collections;

public class FakeCameraZoom : MonoBehaviour
{
    [Header("目标位置")]
    public Vector3 targetPosition;

    [Header("目标缩放")]
    public Vector3 targetScale = Vector3.one * 1.5f;

    [Header("动画时间")]
    public float duration = 1f;

    private Vector3 originalPosition;
    private Vector3 originalScale;

    private bool zoomed;

    void Start()
    {
        originalPosition = transform.localPosition;
        originalScale = transform.localScale;
    }

    public void PlayZoom()
    {
        StopAllCoroutines();

        if (!zoomed)
        {
            StartCoroutine(Animate(
                targetPosition,
                targetScale
            ));
        }
        else
        {
            StartCoroutine(Animate(
                originalPosition,
                originalScale
            ));
        }

        zoomed = !zoomed;
    }

    IEnumerator Animate(
        Vector3 endPos,
        Vector3 endScale)
    {
        Vector3 startPos = transform.localPosition;
        Vector3 startScale = transform.localScale;

        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;

            transform.localPosition =
                Vector3.Lerp(startPos, endPos, t);

            transform.localScale =
                Vector3.Lerp(startScale, endScale, t);

            yield return null;
        }

        transform.localPosition = endPos;
        transform.localScale = endScale;
    }
}
