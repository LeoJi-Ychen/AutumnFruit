using UnityEngine;

public class Effect_CameraScale : MonoBehaviour
{
    public Camera cam;
    public float clock;
    float startScale;
    public float finalScale;
    public Effect effect;
    public float delay_time;
    int state;
    float timer;
    float wait_timer;
    float effectState;
    public enum Effect
    {
        Directly,
        Delayed
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        if(effect == Effect.Directly)
        {
            startScale = cam.orthographicSize;
            state = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (effectState == 1)
        {
            this.enabled = false;
        }
        if (state == 0)
        {
            wait_timer += Time.deltaTime;
            if(wait_timer > delay_time)
            {
                startScale = cam.orthographicSize;
                state = 1;
            } 
        }
        if (state == 1)
        {
            timer += Time.deltaTime;
            if (timer > clock)
            {
                timer = clock;
                effectState = 1;
            }
            cam.orthographicSize = Mathf.SmoothStep(startScale, finalScale, timer / clock);
        }
    }
}
