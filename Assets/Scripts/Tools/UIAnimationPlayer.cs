using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
public class UIAnimationPlayer : IAnim
{
    public float frameTime = 0.01f;
    public float timer;
    public int frame;
    public List<Sprite> sprites = new List<Sprite>();
    public List<int> length = new List<int>();

    public void AnimPlay(GameObject obj, int id, float dt)
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
    public void SetSprites(string route)
    {
        Sprite[] array = Resources.LoadAll<Sprite>(route);

        if (array.Length == 0)
        {
            length.Add(array.Length + sprites.Count);
            return;
        }
        // °´Ãû³ÆÅÅÐò£¨A¡úZ£©
        length.Add(array.Length + sprites.Count);
        sprites.AddRange(array.OrderBy(s => s.name));
    }
    public void SetFrameTime(float t)
    {
        frameTime = t;
    }
}