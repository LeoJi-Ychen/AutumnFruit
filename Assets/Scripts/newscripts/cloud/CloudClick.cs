using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CloudClick : MonoBehaviour, IPointerClickHandler
{
    [Header("Animator")]
    public Animator animator;

    public Action onCloudClicked;

    private bool isClicked = false;
    private bool isWaitingDestroy = false;

    void Awake()
    {
        if (animator != null)
        {
            animator.enabled = false;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isClicked) return;

        isClicked = true;

        if (animator != null)
        {
            animator.enabled = true;
            isWaitingDestroy = true;
        }
        else
        {
            Destroy(gameObject);
        }

        onCloudClicked?.Invoke();
    }

    void Update()
    {
        if (!isWaitingDestroy || animator == null) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.normalizedTime >= 1f)
        {
            Destroy(gameObject);
        }
    }
}