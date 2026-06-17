using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class KeyboardCommand : MonoBehaviour
{
    public List<GameObject> next = new();
    public UnityEvent onAllComplete;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            foreach(GameObject n in next)
            {
                if (n != null)
                {
                    n.SetActive(true);
                }
            }
            onAllComplete?.Invoke();
        }
    }
}
