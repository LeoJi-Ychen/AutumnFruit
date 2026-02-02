using UnityEngine;
using UnityEngine.UI;
public class Button_Audio : MonoBehaviour
{
    public GameObject audioSource;
    public effect e;
    public int jumpid;
    public bool process_state;
    AudioEffect ae;
    public enum effect
    {
        jump,
        next,
        last,
        stop,
        play,
        startloop,
        closeloop,
        toggleloop,
        stoporplay
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ae= audioSource.GetComponent<AudioEffect>();
        if (e == effect.jump)
        {
            GetComponent<Button>().onClick.AddListener(() => ae.JumpTo(jumpid, process_state));
        }
        if (e == effect.next)
        {
            GetComponent<Button>().onClick.AddListener(() => ae.NextClip(process_state));
        }
        if (e == effect.last)
        {
            GetComponent<Button>().onClick.AddListener(() => ae.LastClip(process_state));
        }
        if (e == effect.stop)
        {
            GetComponent<Button>().onClick.AddListener(() => ae.Stop());
        }
        if (e == effect.play)
        {
            GetComponent<Button>().onClick.AddListener(() => ae.Play());
        }
        if (e == effect.startloop)
        {
            GetComponent<Button>().onClick.AddListener(() => ae.StartLooping());
        }
        if (e == effect.closeloop)
        {
            GetComponent<Button>().onClick.AddListener(() => ae.StopLooping());
        }
        if (e == effect.toggleloop)
        {
            GetComponent<Button>().onClick.AddListener(() => ae.ToggleLoop());
        }
        if (e == effect.stoporplay)
        {
            GetComponent<Button>().onClick.AddListener(() => ae.StopOrPlay());
        }
    }
}
