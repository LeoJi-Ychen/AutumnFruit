using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Effect_Anim : MonoBehaviour
{
    public List<string> routes= new List<string>();
    public float frameTime;
    public int anim_index;
    public PlayMode playMode;
    AnimObject animObject;
    public enum PlayMode
    {
        RealTime,
        UnscaleTime
    }
    public enum AnimObject
    {
        UI,
        GameObject
    }

    float timer;
    int frame;
    List<Sprite> sprites = new List<Sprite>();
    List<int> length = new List<int>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GetComponent<Image>())
        {
            animObject = AnimObject.UI;
        }
        else
        {
            animObject = AnimObject.GameObject;
        }
        if (frameTime <= 0)
        {
            frameTime = 0.01f;
        }
        foreach(string r in routes)
        {
            SetSprites(r);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sprites.Count > 0)
        {
            if (animObject == AnimObject.UI)
            {
                if (playMode == PlayMode.RealTime)
                {
                    AnimPlay_UI(this.gameObject, anim_index, Time.deltaTime);
                }
                else if (playMode == PlayMode.UnscaleTime)
                {
                    AnimPlay_UI(this.gameObject, anim_index, Time.unscaledDeltaTime);
                }
            }
            else if (animObject == AnimObject.GameObject)
            {
                if (playMode == PlayMode.RealTime)
                {
                    AnimPlay_GameObject(this.gameObject, anim_index, Time.deltaTime);
                }
                else if (playMode == PlayMode.UnscaleTime)
                {
                    AnimPlay_GameObject(this.gameObject, anim_index, Time.unscaledDeltaTime);
                }
            }
        }      
    }
    public void AnimPlay_UI(GameObject obj, int id, float dt)
    {
        timer += dt;
        if (timer > frameTime)
        {
            if (id == 0)
            {
                frame += Mathf.CeilToInt(timer / frameTime);
                if (frame > length[id] - 1)
                {
                    frame = 0;
                }
            }
            else
            {
                if (frame < length[id - 1])
                {
                    frame = length[id - 1];
                }
                frame += Mathf.CeilToInt(timer / frameTime);
                if (frame > length[id] - 1)
                {
                    frame = length[id - 1];
                }
            }
            timer = 0;
        }
        if (sprites[frame] != null)
        {
            obj.GetComponent<Image>().sprite = sprites[frame];
        }
    }
    public void AnimPlay_GameObject(GameObject obj, int id, float dt)
    {
        timer += dt;
        if (timer > frameTime)
        {
            if (id == 0)
            {
                frame += Mathf.CeilToInt(timer / frameTime);
                if (frame > length[id] - 1)
                {
                    frame = 0;
                }
            }
            else
            {
                if (frame < length[id - 1])
                {
                    frame = length[id - 1];
                }
                frame += Mathf.CeilToInt(timer / frameTime);
                if (frame > length[id] - 1)
                {
                    frame = length[id - 1];
                }
            }
            timer = 0;
        }
        if (sprites[frame] != null)
        {
            obj.GetComponent<SpriteRenderer>().sprite = sprites[frame];
        }
    }
    public void SetSprites(string route)
    {
        Sprite[] array = Resources.LoadAll<Sprite>(route);

        if (array.Length == 0)
        {
            length.Add(array.Length + sprites.Count);
            return;
        }
        length.Add(array.Length + sprites.Count);
        sprites.AddRange(array.OrderBy(s => s.name));
    }
}
