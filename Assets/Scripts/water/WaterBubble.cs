using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class WaterBubble : MonoBehaviour
{
    public WaterGameplay gameplay;
    public int state;
    public float time_1;
    public float time_2;
    public float end_time;
    float timer;
    public RuntimeAnimatorController runtimeAnimatorController;
    public string idleName;
    public string playName;
    Animator anim;
    int anim_state;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init()
    {
        if (!GetComponent<Button>())
        {
            this.gameObject.AddComponent<Button>();
            GetComponent<Animator>();
        }
        if (!GetComponent<Animator>())
        {
            this.gameObject.AddComponent<Animator>();
        }
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = runtimeAnimatorController;
        GetComponent<Button>().onClick.AddListener(Click);
    }
    private void OnEnable()
    {
        timer = 0;
        state = 0;
        anim_state = 0;
        anim.Play(idleName);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 1)
        {
            if (anim_state == 0)
            {
                anim_state = 1;
                anim.Play(playName);
            }
            timer += Time.deltaTime;   
            if(timer >= end_time)
            {
                timer = 0;
                gameplay.LoseGame();
            }
        }
    }
    void Click()
    {
        if (state == 1)
        {
            if (timer >= time_1 && timer <= time_2)
            {
                state = 0;
                timer = 0;
                anim.speed = 0;
                anim.Play(idleName);
                gameplay.NextStep();
            }
        }          
    }
}
