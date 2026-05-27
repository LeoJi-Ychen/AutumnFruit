using System.Collections.Generic;
using UnityEngine;
public interface IAnim
{
    void AnimPlay(GameObject obj,int id, float dt);
    void SetSprites(string route);
    void SetFrameTime(float t);
}
