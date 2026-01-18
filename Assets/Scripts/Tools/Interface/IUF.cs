using System.Collections.Generic;
using UnityEngine;

public interface IUF
{
    float SQ(float a);
    int SQ(int a);
    float Max(float a, float b, float c);
    float Min(float a, float b, float c);
    float ValueBetween(float v, float min, float max);
    int ValueBetween(int v, int min, int max);
    Vector2 Direction2(GameObject me, GameObject aim);
    Vector2 Direction2(GameObject me, Vector2 aim);
    Vector2 Direction2(Vector2 me, Vector2 aim);
    float Distance2(GameObject a, GameObject b);
    float Distance2(GameObject a, Vector2 b);
    float Distance2(Vector2 a, Vector2 b);
    Vector3 Direction3(GameObject me, GameObject aim);
    Vector3 Direction3(GameObject me, Vector3 aim);
    Vector3 Direction3(Vector3 me, Vector3 aim);
    float Distance3(GameObject a, GameObject b);
    float Distance3(GameObject a, Vector3 b);
    float Distance3(Vector3 a, Vector3 b);
    bool RandomRes(float x);
    Vector2 RotatedVector2(Vector2 initVector, float angleDegrees);
    Vector2 GenerateRandomUnitVector2();
    Vector2 GenerateRandomVector2WithinAngle(Vector2 originalVector, float angleRange);
    float angle_radian(float angle);

    void LoadAllResources<T>(string route, List<T> list) where T : UnityEngine.Object;
    void ReadFile(string route, Component T);
    void WriteFile(string route, Component T);
    float Remap(float value, float from1, float to1, float from2, float to2);
    void SaveStructToJson<T>(T data, string filePath);
    void LoadStructFromJson<T>(ref T data, string filePath);
    T LoadStructFromJson<T>(string filePath);
    T LoadResource<T>(string route, int id) where T : UnityEngine.Object;
    void AddText(GameObject obj, string content);

    void MoveByKey(GameObject role, float speed, int state);
    void MoveByMouse(GameObject role, Vector3 offset, float speed);
    void MoveToMouse(GameObject role, float speed);
    int GetKeyState();
    void MoveLimitation(GameObject role, float width, float height,Vector2 center);
    void FaceToMoveDir(GameObject obj,GameObject avatarFace,int mode=0);//0-初始朝右,1-初始朝左
    void UpLookAtMoveDir(GameObject obj, GameObject avatar, float speed);
    void UpLookAtMouse(GameObject obj, GameObject avatar, float speed);
    void UpRotateByMouse(GameObject obj, GameObject avatar, float maxdis, float speed);
    void ObjMoveTo(GameObject obj, Vector2 pos, float speed);
    void ObjRotateByCenter(GameObject obj, Vector2 dir, float theta, float t);
    void ObjRotateByCenterByMouse(GameObject obj, GameObject avatar, float theta, float maxdis);
    void ObjRotateByCenterByMouseX(GameObject obj, GameObject avatar, float theta, float maxdis);
    void ObjRotateByCenterByMouseY(GameObject obj, GameObject avatar, float theta, float maxdis);
    void ObjRotateAroundZ(GameObject obj, float angle,int mode=0);//0-世界 1-自身坐标系 
    Vector3 toVec3(Vector2 v, float z = 0);
    Vector3 GetOffsetOfMouse(GameObject obj);
    Rect Area(GameObject obj);
    bool InArea(Vector2 pos,Rect r);
    Vector2 MouseWorldPos();
    void EraseTexture(GameObject obj, float range, int mode = 0);//0-擦除模式0
}
