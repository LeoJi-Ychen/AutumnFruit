using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class Plane : MonoBehaviour
{
    
    [Header("注:只用绑定这一个组件就可以了")]
    [Header("飞机大战游戏：将飞机，石头星星等放置成这个对象的子对象")]
    public GameObject game;
    [Header("游戏胜利后激活这个游戏会setfalse,这个next对象会激活，可以把镜头移动的触发组件绑到这个next上")]
    public GameObject next;
    [Header("胜利所需的星星数")]
    public int starNeeded;   
    [Header("飞机的Image")]
    public GameObject plane;
    [Header("飞机的子弹")]
    public GameObject projection;
    [Header("石头的Image")]
    public GameObject stone;
    [Header("星星的Image")]
    public GameObject star;
    [Header("判定触发距离")]
    public float triggerDistance;
    [Header("飞机速度")]
    public float planeSpeed;
    [Header("子弹速度")]
    public float projectionSpeed;
    [Header("石头速度:会在min与max间随机取值")]
    public float stonespeed_min;
    public float stonespeed_max;
    [Header("星星速度:会在min与max间随机取值")]
    public float starspeed_min;
    public float starspeed_max;
    [Header("星星石头生成去左上点")]
    public GameObject generator_lefttop;
    [Header("星星石头生成去右下点")]
    public GameObject generator_rightdown;
    [Header("飞机移动区域左上点")]
    public GameObject battlearea_lefttop;
    [Header("飞机移动区域右下点")]
    public GameObject battlearea_rightdown;
    [Header("最左侧边界,一般在左侧屏幕外")]
    public GameObject edge;

    [Header("计分显示")]
    public TextMeshProUGUI display_text;
   
    int currentStar;
   

    List<GameObject> stones = new List<GameObject>();
    List<GameObject> stars = new List<GameObject>();
    List<GameObject> projections = new List<GameObject>();
    public float attackGap;
    float timer_attack;


    private void Awake()
    {
        stone.SetActive(false);
        stone.AddComponent<Plane_Stone>();
        star.SetActive(false);
        star.AddComponent<Plane_Star>();
        projection.SetActive(false);
        projection.AddComponent<Plane_Projection>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackGap = 1;
        if (triggerDistance < 0.01f)
        {
            triggerDistance = 1.0f;
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (game == null)
        {
            game = this.gameObject;
        }
        plane.AddComponent<Plane_Move>();
        plane.GetComponent<Plane_Move>().speed = planeSpeed;
        for(int i = 0; i < 5; i++)
        {
            GameObject g = GameObject.Instantiate(stone, game.transform);
            g.transform.position = GeneratePos();
            g.GetComponent<Plane_Stone>().speed = Random.Range(stonespeed_min, stonespeed_max);
            g.SetActive(true);
            stones.Add(g);
        }
        for (int i = 0; i < 3; i++)
        {
            GameObject g = GameObject.Instantiate(star, game.transform);
            g.transform.position = GeneratePos();
            g.GetComponent<Plane_Star>().speed = Random.Range(starspeed_min, starspeed_max);
            g.SetActive(true);
            stars.Add(g);
        }
        for (int i = 0; i < 20; i++)
        {
            GameObject g = GameObject.Instantiate(projection, game.transform);
            g.transform.position = GeneratePos();
            g.GetComponent<Plane_Projection>().speed = projectionSpeed;
            g.SetActive(true);
            projections.Add(g);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer_attack += Time.deltaTime;
        if (timer_attack > attackGap)
        {
            timer_attack = 0;
            Attack();
        }
        foreach(GameObject g in stones)
        {
            if (Distance(plane, g) < triggerDistance)
            {
                g.transform.position = GeneratePos();
                g.GetComponent<Plane_Stone>().speed = Random.Range(stonespeed_min, stonespeed_max);
                currentStar--;
            }
            if (g.transform.position.x < edge.transform.position.x)
            {
                g.transform.position = GeneratePos();
                g.GetComponent<Plane_Stone>().speed = Random.Range(stonespeed_min, stonespeed_max);
            }
        }
        foreach (GameObject g in stars)
        {
            if (Distance(plane, g) < triggerDistance)
            {
                g.transform.position = GeneratePos();
                g.GetComponent<Plane_Star>().speed = Random.Range(starspeed_min, starspeed_max);
                currentStar++;
            }
            if (g.transform.position.x < edge.transform.position.x)
            {
                g.transform.position = GeneratePos();
                g.GetComponent<Plane_Star>().speed = Random.Range(starspeed_min, starspeed_max);
            }
        }
        foreach(GameObject p in projections)
        {
            if (p.GetComponent<Plane_Projection>().state == 1)
            {
                foreach (GameObject g in stones)
                {
                    if (Distance(p, g) < triggerDistance)
                    {
                        p.GetComponent<Plane_Projection>().state = 0;
                        p.transform.position = GeneratePos();
                        g.transform.position = GeneratePos();
                        g.GetComponent<Plane_Stone>().speed = Random.Range(stonespeed_min, stonespeed_max);
                        break;
                    }                 
                }
                if (p.transform.position.x > generator_lefttop.transform.position.x)
                {
                    p.GetComponent<Plane_Projection>().state = 0;
                    p.transform.position = GeneratePos();
                }
            }     
        }
        float edge_left = battlearea_lefttop.transform.position.x;
        float edge_right = battlearea_rightdown.transform.position.x;
        float edge_top = battlearea_lefttop.transform.position.y;
        float edge_down = battlearea_rightdown.transform.position.y;
        if (plane.transform.position.x > edge_right)
        {
            plane.transform.position = new Vector3(edge_right,plane.transform.position.y, plane.transform.position.z);
        }
        if (plane.transform.position.x < edge_left)
        {
            plane.transform.position = new Vector3(edge_left, plane.transform.position.y, plane.transform.position.z);
        }
        if (plane.transform.position.y > edge_top)
        {
            plane.transform.position = new Vector3(plane.transform.position.x,edge_top, plane.transform.position.z);
        }
        if (plane.transform.position.y < edge_down)
        {
            plane.transform.position = new Vector3(plane.transform.position.x, edge_down, plane.transform.position.z);
        }
        if (currentStar < 0)
        {
            currentStar = 0;
        }
        if (currentStar >= starNeeded)
        {
            currentStar = starNeeded;
            Win();
        }
        if (display_text != null)
        {
            display_text.text = currentStar+"/"+starNeeded;
        }
    }
    void Attack()
    {
        foreach(GameObject p in projections)
        {
            if(p.GetComponent<Plane_Projection>().state == 0)
            {
                p.GetComponent<Plane_Projection>().state = 1;
                p.transform.position = plane.transform.position;
                break;
            }
        }
    }
    void Win()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (next != null)
        {
            next.SetActive(true);
        }
        game.SetActive(false);
    }
    float Distance(GameObject a,GameObject b)
    {
        return ((Vector2)a.transform.position - (Vector2)b.transform.position).magnitude;
    }
    Vector2 GeneratePos()
    {
        float x = Random.Range(generator_lefttop.transform.position.x, generator_rightdown.transform.position.x);
        float y = Random.Range(generator_rightdown.transform.position.y, generator_lefttop.transform.position.y);
        Vector2 pos = new Vector2(x,y);
        return pos;
    }
}
