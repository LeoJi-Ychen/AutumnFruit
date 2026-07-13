using UnityEngine;

public class StartScreen : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerPrefs.HasKey("UnlockedChapter"))
        {
            StoryData.UnlockedChapter = PlayerPrefs.GetInt("UnlockedChapter");
        }
        else
        {
            PlayerPrefs.SetInt("UnlockedChapter", 2);
            StoryData.UnlockedChapter = 2;
        }
    }
}
