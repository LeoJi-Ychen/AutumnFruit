using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_DynamicSortingOrder : MonoBehaviour
{
    public int minOrder;
    public int maxOrder;
    public GameObject center;
    // Start is called before the first frame update
    void Start()
    {
        if (minOrder <= 0)
        {
            minOrder = 10;
            maxOrder = 14;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > center.transform.position.y)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = Mathf.Min(10,minOrder);
        }
        else
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = Mathf.Max(14, maxOrder);
        }
    }
}
