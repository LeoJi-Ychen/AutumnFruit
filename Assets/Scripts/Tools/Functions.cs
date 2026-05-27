using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.Animations;
using System.IO;
using System.Data;
public class Functions : IUF
{
    private static readonly string[] chineseDigits = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
    private static readonly string[] chineseUnits = { "", "十", "百", "千", "万", "十", "百", "千", "亿" };
    public string ToChinese(int number)
    {
        if (number == 0) return "零";

        string result = "";
        string numStr = number.ToString();
        int length = numStr.Length;

        for (int i = 0; i < length; i++)
        {
            int digit = numStr[i] - '0';
            int unitIndex = length - 1 - i;
            if (digit != 0)
            {
                result += chineseDigits[digit] + chineseUnits[unitIndex];
            }
            else
            {
                // 处理零：避免连续多个零，且万位、亿位要保留
                if (!result.EndsWith("零") && unitIndex != 4 && unitIndex != 8)
                {
                    result += "零";
                }
                // 万位和亿位特殊处理
                if (unitIndex == 4) result += "万";
                if (unitIndex == 8) result += "亿";
            }
        }
        // 清理末尾的零
        result = result.TrimEnd('零');
        // 处理"一十"开头简化为"十"
        if (result.StartsWith("一十"))
        {
            result = result.Substring(1);
        }
        return result;
    }
    public float SQ(float value)
    {
        return value * value;
    }
    public int SQ(int value)
    {
        return value * value;
    }
    public float Max(float a, float b, float c)
    {
        return Mathf.Max(Mathf.Max(a,b),c);
    }
    public float Min(float a, float b, float c)
    {
        return Mathf.Min(Mathf.Min(a, b), c);
    }
    public float ValueBetween(float v, float min, float max)
    {
        return Mathf.Min(Mathf.Max(v, min), max);
    }
    public int ValueBetween(int v, int min, int max)
    {
        return Mathf.Min(Mathf.Max(v, min), max);
    }
    public Vector2 Direction2(GameObject me, GameObject aim)
    {
        return ((Vector2)aim.transform.position - (Vector2)me.transform.position).normalized;
    }
    public Vector2 Direction2(GameObject me, Vector2 aim)
    {
        return (aim - (Vector2)me.transform.position).normalized;
    }
    public Vector2 Direction2(Vector2 me, Vector2 aim)
    {
        return (aim - me).normalized;
    }
    public float Distance2(GameObject a, GameObject b)
    {
        float dis = 0;
        dis = ((Vector2)a.transform.position - (Vector2)b.transform.position).magnitude;
        return dis;
    }
    public float Distance2(GameObject a, Vector2 b)
    {
        float dis = 0;
        dis = ((Vector2)a.transform.position - b).magnitude;
        return dis;
    }
    public float Distance2(Vector2 a, Vector2 b)
    {
        float dis = 0;
        dis = (a - b).magnitude;
        return dis;
    }
    public Vector3 Direction3(GameObject me, GameObject aim)
    {
        return (aim.transform.position - me.transform.position).normalized;
    }
    public Vector3 Direction3(GameObject me, Vector3 aim)
    {
        return (aim - me.transform.position).normalized;
    }
    public Vector3 Direction3(Vector3 me, Vector3 aim)
    {
        return (aim - me).normalized;
    }
    public float Distance3(GameObject a, GameObject b)
    {
        float dis = 0;
        dis = (a.transform.position - b.transform.position).magnitude;
        return dis;
    }
    public float Distance3(GameObject a, Vector3 b)
    {
        float dis = 0;
        dis = (a.transform.position - b).magnitude;
        return dis;
    }
    public float Distance3(Vector3 a, Vector3 b)
    {
        float dis = 0;
        dis = (a - b).magnitude;
        return dis;
    }
    public bool RandomRes(float x)
    {
        if (x <= 0)
        {
            return false;
        }
        float randomValue = Random.value;
        return randomValue < x ? true : false;
    }
    public Vector2 RotatedVector2(Vector2 initVector, float angleDegrees)
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, angleDegrees);
        Vector2 rotatedVector = rotation * initVector;
        return rotatedVector;
    }
    public Vector2 GenerateRandomUnitVector2()
    {
        float angle = Random.Range(0f, 360f);
        float radians = angle_radian(angle);
        float x = Mathf.Cos(radians);
        float y = Mathf.Sin(radians);
        return new Vector2(x, y);
    }
    public Vector2 GenerateRandomVector2WithinAngle(Vector2 originalVector, float angleRange)
    {
        float randomAngle = Random.Range(-angleRange, angleRange);
        Vector2 rotatedVector = RotatedVector2(originalVector, randomAngle);
        return rotatedVector;
    }
    public float angle_radian(float angle)
    {
        return angle * Mathf.PI / 180f;
    }

    public void LoadAllResources<T>(string route, List<T> list) where T : UnityEngine.Object
    {
        list.Clear();
        // Resources ¼ÓÔØÈ«²¿
        T[] array = Resources.LoadAll<T>(route);

        if (array.Length == 0)
        {
            return;
        }
        // °´Ãû³ÆÅÅÐò£¨A¡úZ£©
        list.AddRange(array.OrderBy(s => s.name));
    }
    public void ReadFile(string route, Component T)
    {
        string name = route;
        string path = Application.dataPath + @"/StreamingAssets/" + name + ".json";
        if (File.Exists(path))
        {
            StreamReader sr = new StreamReader(path);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            JsonUtility.FromJsonOverwrite(jsonStr, T);
        }
    }
    public void WriteFile(string route, Component T)
    {
        string name = route;
        string path = Application.dataPath + @"/StreamingAssets/" + name + ".json";
        string saveJsonStr = JsonUtility.ToJson(T, true);
        StreamWriter sw = new StreamWriter(path);
        sw.Write(saveJsonStr);
        sw.Close();
    }
    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    public void SaveStructToJson<T>(T data, string filePath)
    {
        string path = Application.dataPath + @"/StreamingAssets/" + filePath + ".json";
        // ½«structÐòÁÐ»¯ÎªJSON×Ö·û´®
        string jsonString = JsonUtility.ToJson(data);

        // ½«JSON×Ö·û´®Ð´ÈëÎÄ¼þ
        File.WriteAllText(path, jsonString);
    }
    public void LoadStructFromJson<T>(ref T data, string filePath)
    {
        string path = Application.dataPath + @"/StreamingAssets/" + filePath + ".json";
        // ´ÓÎÄ¼þ¶ÁÈ¡JSON×Ö·û´®
        string jsonString = File.ReadAllText(path);

        // ½«JSON×Ö·û´®·´ÐòÁÐ»¯Îªstruct
        data = JsonUtility.FromJson<T>(jsonString);
    }
    public T LoadStructFromJson<T>(string filePath)
    {
        string path = Application.dataPath + @"/StreamingAssets/" + filePath + ".json";
        // ´ÓÎÄ¼þ¶ÁÈ¡JSON×Ö·û´®
        string jsonString = File.ReadAllText(path);

        // ½«JSON×Ö·û´®·´ÐòÁÐ»¯Îªstruct
        T data = JsonUtility.FromJson<T>(jsonString);

        return data;
    }
    public T LoadResource<T>(string route, int id) where T : UnityEngine.Object
    {
        // Resources ¼ÓÔØÈ«²¿
        List<T> tlist = new List<T>();
        T[] array = Resources.LoadAll<T>(route);

        if (array.Length == 0)
        {
            return null;
        }
        // °´Ãû³ÆÅÅÐò£¨A¡úZ£©
        tlist.AddRange(array.OrderBy(s => s.name));
        return tlist[id];
    }
    public T LoadResource<T>(string route, string name) where T : UnityEngine.Object
    {
        T[] array = Resources.LoadAll<T>(route);

        if (array.Length == 0)
        {
            return null;
        }

        return array.FirstOrDefault(s => s.name == name);
    }
    public void AddText(GameObject obj, string content)
    {
        obj.GetComponent<TextMeshProUGUI>().text = content;
    }

    public void MoveByKey(GameObject role, float speed, int state)
    {
        if (state == 1)
        {
            role.GetComponent<Rigidbody2D>().linearVelocity = speed * Vector2.up;
        }
        else if (state == 2)
        {
            role.GetComponent<Rigidbody2D>().linearVelocity = speed * Vector2.left;
        }
        else if (state == 3)
        {
            role.GetComponent<Rigidbody2D>().linearVelocity = speed * Vector2.down;
        }
        else if (state == 4)
        {
            role.GetComponent<Rigidbody2D>().linearVelocity = speed * Vector2.right;
        }
        else if (state == 5)
        {
            role.GetComponent<Rigidbody2D>().linearVelocity = speed * (Vector2.up + Vector2.left).normalized;
        }
        else if (state == 6)
        {
            role.GetComponent<Rigidbody2D>().linearVelocity = speed * (Vector2.up + Vector2.right).normalized;
        }
        else if (state == 7)
        {
            role.GetComponent<Rigidbody2D>().linearVelocity = speed * (Vector2.down + Vector2.left).normalized;
        }
        else if (state == 8)
        {
            role.GetComponent<Rigidbody2D>().linearVelocity = speed * (Vector2.down + Vector2.right).normalized;
        }
        if (state == 0)
        {
            role.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }
    public void MoveByMouse(GameObject role, Vector3 offset, float speed)
    {
        Vector2 aimPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        if (speed < 0)
        {
            role.transform.position = aimPos;
        }
        else
        {
            if (Distance2(role, aimPos) < 0.02f * speed)
            {
                role.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            }
            else
            {
                role.GetComponent<Rigidbody2D>().linearVelocity = Direction2(role, aimPos) * speed;
            }
        }      
    }
    public void MoveToMouse(GameObject role, float speed)
    {
        Vector2 aimPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Distance2(role, aimPos) < 0.02f*speed)
        {
            role.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
        else
        {
            role.GetComponent<Rigidbody2D>().linearVelocity = Direction2(role, aimPos) * speed;
        }
    }
    public int GetKeyState()
    {
        int buttonState = 0;
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            buttonState = 8;
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            buttonState = 7;
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            buttonState = 6;
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            buttonState = 5;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            buttonState = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            buttonState = 3;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            buttonState = 2;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            buttonState = 1;
        }
        else
        {
            buttonState = 0;
        }
        return buttonState;
    }
    public void MoveLimitation(GameObject role, float width, float height,Vector2 center)
    {
        if (role.transform.position.x < -width / 2)
        {
            role.transform.position = new Vector3(center.x-width / 2, role.transform.position.y, 0);
        }
        if (role.transform.position.x > width / 2)
        {
            role.transform.position = new Vector3(center.x+width / 2, role.transform.position.y, 0);
        }
        if (role.transform.position.y < -height / 2)
        {
            role.transform.position = new Vector3(role.transform.position.x, center.y-height / 2, 0);
        }
        if (role.transform.position.y > height / 2)
        {
            role.transform.position = new Vector3(role.transform.position.x, center.y+height / 2, 0);
        }
    }
    public void FaceToMoveDir(GameObject obj,GameObject avatarFace, int mode = 0)
    {
        float vx = obj.GetComponent<Rigidbody2D>().linearVelocityX;
        if (mode == 0)
        {
            if (vx > 0)
            {
                avatarFace.transform.localScale = new Vector3(Mathf.Abs(avatarFace.transform.localScale.x), avatarFace.transform.localScale.y, 1);
            }
            if (vx < 0)
            {
                avatarFace.transform.localScale = new Vector3(-Mathf.Abs(avatarFace.transform.localScale.x), avatarFace.transform.localScale.y, 1);
            }
        }
        if (mode == 1)
        {
            if (vx > 0)
            {
                avatarFace.transform.localScale = new Vector3(-Mathf.Abs(avatarFace.transform.localScale.x), avatarFace.transform.localScale.y, 1);
            }
            if (vx < 0)
            {
                avatarFace.transform.localScale = new Vector3(Mathf.Abs(avatarFace.transform.localScale.x), avatarFace.transform.localScale.y, 1);
            }
        }
    }
    public void UpLookAtMoveDir(GameObject obj, GameObject avatar, float speed)
    {
        Vector2 v = obj.GetComponent<Rigidbody2D>().linearVelocity;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, toVec3(v));
        avatar.transform.rotation = Quaternion.RotateTowards(avatar.transform.rotation, rotation, speed * Time.deltaTime);
    }
    public void UpLookAtMouse(GameObject obj, GameObject avatar, float speed)
    {
        Vector3 p0 = obj.transform.position;
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = Direction2(p0, mp);
        float dis = Distance2(mp, p0);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up,toVec3(dir));
        avatar.transform.rotation = Quaternion.RotateTowards(avatar.transform.rotation, rotation, speed*Time.deltaTime);
    }
    public void UpRotateByMouse(GameObject obj, GameObject avatar, float maxdis,float speed)
    {
        Vector3 p0 = obj.transform.position;
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = Direction2(p0, mp);
        float dis = Distance2(mp, p0);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, toVec3(dir));
        avatar.transform.rotation =Quaternion.RotateTowards(avatar.transform.rotation, Quaternion.Slerp(Quaternion.identity, rotation, dis/maxdis),speed*Time.deltaTime);
    }
    public void ObjMoveTo(GameObject obj, Vector2 pos,float speed)
    {
        if (Distance2(obj, pos) < 0.02f*speed)
        {
            obj.transform.position = pos;
        }
        else
        {
            obj.transform.position += toVec3(Direction2((Vector2)obj.transform.position, pos) * speed * Time.deltaTime);
        }
    }
    public void ObjRotateByCenter(GameObject obj, Vector2 dir, float theta, float t)
    {
        Quaternion q = Quaternion.FromToRotation(Vector3.forward, new Vector3(dir.x, dir.y, Mathf.Tan(angle_radian(90-theta))).normalized);
        obj.transform.rotation = Quaternion.Slerp(Quaternion.identity, q, t);
    }
    public void ObjRotateByCenterByMouse(GameObject obj, GameObject avatar, float theta, float maxdis)
    {
        Vector3 p0 = obj.transform.position;
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = Direction2(p0, mp);
        float dis = Distance2(mp, p0);
        if (dis < 0.01*maxdis)
        {
            avatar.transform.rotation = Quaternion.identity;
        }
        else
        {
            Quaternion q = Quaternion.FromToRotation(Vector3.forward, new Vector3(dir.x, dir.y, Mathf.Tan(angle_radian(90 - theta))).normalized);
            avatar.transform.rotation = Quaternion.Slerp(Quaternion.identity, q, Mathf.Min(dis / maxdis, 1));
        }
    }
    public void ObjRotateByCenterByMouseX(GameObject obj, GameObject avatar, float theta, float maxdis)
    {
        Vector3 p0 = obj.transform.position;
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = Direction2(p0, mp);
        float dis = Distance2(mp, p0);
        if (dis < 0.01 * maxdis)
        {
            avatar.transform.rotation = Quaternion.identity;
        }
        else
        {
            Quaternion q = Quaternion.FromToRotation(Vector3.forward, new Vector3(dir.x, 0, Mathf.Tan(angle_radian(90 - theta))).normalized);
            avatar.transform.rotation = Quaternion.Slerp(Quaternion.identity, q, Mathf.Min(dis / maxdis, 1));
        }
    }
    public void ObjRotateByCenterByMouseY(GameObject obj, GameObject avatar, float theta, float maxdis)
    {
        Vector3 p0 = obj.transform.position;
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = Direction2(p0, mp);
        float dis = Distance2(mp, p0);
        if (dis < 0.01 * maxdis)
        {
            avatar.transform.rotation = Quaternion.identity;
        }
        else
        {
            Quaternion q = Quaternion.FromToRotation(Vector3.forward, new Vector3(0, dir.y, Mathf.Tan(angle_radian(90 - theta))).normalized);
            avatar.transform.rotation = Quaternion.Slerp(Quaternion.identity, q, Mathf.Min(dis / maxdis, 1));
        }
    }
    public void ObjRotateAroundZ(GameObject obj, float angle,int mode=0)
    {
        if (mode == 0)
        {
            Vector3 axis = Vector3.forward;
            Quaternion rotation = Quaternion.AngleAxis(angle, axis);
            obj.transform.rotation = obj.transform.rotation * rotation;
        }
        else if (mode == 1)
        {
            Vector3 axis = obj.transform.forward;
            Quaternion rotation = Quaternion.AngleAxis(angle, axis);
            obj.transform.rotation = obj.transform.rotation * rotation;
        }      
    }
    public Vector3 toVec3(Vector2 v, float z = 0)
    {
        return new Vector3(v.x, v.y, z);
    }
    public Vector3 GetOffsetOfMouse(GameObject obj)
    {
        return obj.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public Rect Area(GameObject obj)
    {
        Rect rect = new Rect(obj.transform.position.x, obj.transform.position.y, obj.GetComponent<SpriteRenderer>().sprite.bounds.size.x, obj.GetComponent<SpriteRenderer>().sprite.bounds.size.y);
        return rect;
    }
    public bool InArea(Vector2 pos, Rect r)
    {
        if (pos.x > (r.x - r.width / 2))
        {
            if (pos.x < (r.x + r.width / 2))
            {
                if (pos.y > (r.y - r.height / 2))
                {
                    if (pos.y < (r.y + r.height / 2))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public Vector2 MouseWorldPos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public void EraseTexture(GameObject obj, float range, int mode=0)
    {
        Sprite image = obj.GetComponent<SpriteRenderer>().sprite;
        int width = image.texture.width;
        int height = image.texture.height;
        Color[] colors = new Color[width * height];
        Texture2D texture = image.texture;
        Texture2D temporaryTexture = new Texture2D(texture.width, texture.height, texture.format, false);
        temporaryTexture.SetPixels(texture.GetPixels());
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colors[y * width + x] = temporaryTexture.GetPixel(x, y);
            }
        }
        switch (mode)
        {
            case 0:
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        colors[y * width + x].a -= range * ((float)(y) / (float)height);
                    }
                }
                break;
        }
        temporaryTexture.SetPixels(colors);
        temporaryTexture.Apply();
        obj.GetComponent<SpriteRenderer>().sprite = Sprite.Create(temporaryTexture, obj.GetComponent<SpriteRenderer>().sprite.rect, obj.GetComponent<SpriteRenderer>().sprite.pivot);
    }
    public List<T> KeepIndices<T>(List<T> originalList, List<int> indicesToKeep)
    {
        List<T> result = new List<T>(indicesToKeep.Count);

        foreach (int index in indicesToKeep)
        {
            if (index >= 0 && index < originalList.Count)
            {
                result.Add(originalList[index]);
            }
        }

        return result;
    }
}
