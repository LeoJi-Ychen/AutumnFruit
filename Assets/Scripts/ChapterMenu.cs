using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ChapterMenu : MonoBehaviour
{
    public int id;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (id > StoryData.UnlockedChapter)
        {
            GetComponent<Button>().interactable = false;
            ChangeColor(this.gameObject.transform);
        }
        else
        {
            GetComponent<Button>().interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ChangeColor(Transform t)
    {
        if (t.GetComponent<TextMeshProUGUI>())
        {
            t.GetComponent<TextMeshProUGUI>().color = Color.gray6;
        }
        if (t.transform.childCount <= 0)
        {            
            return;
        }
        for(int i = 0; i < t.transform.childCount; i++)
        {
            ChangeColor(t.GetChild(i));
        }
    }
}
