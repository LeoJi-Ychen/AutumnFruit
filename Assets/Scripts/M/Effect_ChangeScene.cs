using UnityEngine;
using UnityEngine.SceneManagement;
public class Effect_ChangeScene : MonoBehaviour
{
    public string SceneName;
    public Effect effect;
    public float delay_time;
    int state;
    float timer;
    public enum Effect
    {
        Directly,
        Delayed
    }
    private void OnEnable()
    {
        timer = 0;
        state = 0;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 1)
        {
            timer += Time.deltaTime;
            if (timer > delay_time)
            {
                timer = 0;
                state = 0;
                SceneManager.LoadSceneAsync(SceneName);
            }
        }
    }
    void ChangeScene()
    {
        if (effect == Effect.Directly)
        {
            SceneManager.LoadSceneAsync(SceneName);
        }
        else if (effect == Effect.Delayed)
        {
            state = 1;
        }
    }
}
