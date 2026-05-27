using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class BubbleClick : MonoBehaviour, IPointerClickHandler
{
    public float existTime;
    float currentTime;
    public float triggerTime;
    public BubbleClickGameplay gameplay;
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    private void OnEnable()
    {
        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0);
        }    
    }
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime > existTime)
        {
            currentTime = 0;
            gameplay.ResetCount();
            this.gameObject.SetActive(false);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Click();
    }
    void Click()
    {
        if (currentTime >= triggerTime)
        {
            currentTime = 0;
            gameplay.AddCount();
            this.gameObject.SetActive(false);
        }
    }
}
