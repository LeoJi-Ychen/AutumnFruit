using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TranstionEffect : MonoBehaviour
{
    public List<GameObject> buttons = new List<GameObject>();
    public Kind kind;
    public string sceneName;
    public Transform nextPage;
    public GameObject currentPanel;
    public GameObject nextPanel;
    public float Clock;
    public float TransParencyClock;
    public List<GameObject> currentFade = new List<GameObject>();
    public List<GameObject> nextAppear = new List<GameObject>();
    TransparencyController currentTransparencyController;
    TransparencyController nextTransparencyController;
    int state;
    float timer;
    Camera cam;
    Vector3 originPos;
    int isLoading;
    private AsyncOperation preloadOp;
    private string targetScene;
    EventSystem es;

    public enum Kind
    {
        ChangeScene,
        ChangePanel
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        es = EventSystem.current;
        foreach(GameObject button in buttons)
        {
            button.GetComponent<Button>().onClick.AddListener(Transition);
        }
    }
    private void OnEnable()
    {
        state = 0;
        timer = 0;
        if (nextPanel == null)
        {
            nextPanel = nextPage.gameObject;
        }         
    }

    void InitTransparencyController()
    {
        cam = Camera.main;
        originPos = cam.transform.position;
        if (currentTransparencyController == null)
        {
            currentTransparencyController = this.gameObject.AddComponent<TransparencyController>();
            currentTransparencyController.objs = new List<GameObject>(currentFade);
        }
        if (nextTransparencyController == null)
        {
            nextTransparencyController = this.gameObject.AddComponent<TransparencyController>();
            nextTransparencyController.objs = new List<GameObject>(nextAppear);
        }
        nextPanel.SetActive(false);
        nextTransparencyController.transparency = -1;
    }
    // Update is called once per frame
    void Update()
    {
        if (state == 1)
        {
            if(isLoading == 0)
            {
                isLoading = 1;
                if (kind == Kind.ChangeScene)
                {
                    Preload(sceneName);
                }
            }
            if (timer <= Clock)
            {
                timer += Time.deltaTime;
                CameraMoveEffect();
                TransparencyEffect();
            }                 
            if (timer > Clock)
            {
                TransparencyEffectEnd();
                es.enabled = true;
                if (kind == Kind.ChangeScene)
                {
                    if (nextPanel != null)
                    {
                        nextPanel.SetActive(true);
                    }
                    if (currentPanel != null)
                    {
                        currentPanel.SetActive(false);
                    }
                    state = 0;
                    //timer = 0;
                    SwitchToPreloadedScene();
                }
                else
                {     
                    if (nextPanel != null)
                    {
                        nextPanel.SetActive(true);
                    }
                    if (currentPanel != null)
                    {
                        currentPanel.SetActive(false);
                    }
                    state = 0;
                    //timer = 0;
                }
            }
        }             
    }
    public void CameraMoveEffect()
    {
        float x = Mathf.SmoothStep(originPos.x, nextPage.position.x, timer / Clock);
        float y = Mathf.SmoothStep(originPos.y, nextPage.position.y, timer / Clock);
        cam.transform.position = new Vector3(x, y,originPos.z);
    } 
    public void TransparencyEffect()
    {
        if (currentTransparencyController != null)
        {
            currentTransparencyController.state = 1;
            currentTransparencyController.transparency = Mathf.SmoothStep(1, 0, Mathf.Max(0,Mathf.Min(timer / TransParencyClock,1)));
        }
        if (nextTransparencyController != null)
        {
            nextTransparencyController.state = 1;
            nextTransparencyController.transparency = Mathf.SmoothStep(1, 0, Mathf.Max(0, Mathf.Min(((Clock - timer) / TransParencyClock),1)));
        }        
    }
    public void TransparencyEffectEnd()
    {
        if (currentTransparencyController != null)
        {
            currentTransparencyController.state = 0;
            currentTransparencyController.transparency = 0;
        }
        if (nextTransparencyController != null)
        {
            nextTransparencyController.state = 0;
            nextTransparencyController.transparency = 1;
        }
    }
    public void Transition()
    {
        InitTransparencyController();
        if (state == 0)
        {
            nextPanel.SetActive(true);
            originPos = cam.transform.position;
            state = 1;
            timer = 0;
            es.enabled = false;
            //EventSystem.current.enabled = true;
        }
    }
    public void Preload(string sceneName)
    {
        targetScene = sceneName;
        StartCoroutine(PreloadCoroutine());
    }

    private IEnumerator PreloadCoroutine()
    {
        preloadOp = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
        preloadOp.allowSceneActivation = false;

        while (preloadOp.progress < 0.9f)
        {
            Debug.Log($"预加载进度: {preloadOp.progress * 100f:0}%");
            yield return null;
        }

        Debug.Log("预加载完成，等待切换");
    }

    /// 2️⃣ 切换（核心逻辑）
    public void SwitchToPreloadedScene()
    {
        StartCoroutine(SwitchCoroutine());
    }

    private IEnumerator SwitchCoroutine()
    {
        if (preloadOp == null)
        {
            Debug.LogError("没有预加载场景！");
            yield break;
        }

        // ⚠️ 关键：先允许激活
        preloadOp.allowSceneActivation = true;

        // 等新场景真正加载完成
        while (!preloadOp.isDone)
            yield return null;

        Scene newScene = SceneManager.GetSceneByName(targetScene);

        // 设置为激活场景
        SceneManager.SetActiveScene(newScene);

        // 3️⃣ 卸载所有旧场景
        yield return StartCoroutine(UnloadAllExcept(newScene));

        Debug.Log("场景切换完成！");
    }

    /// 卸载除目标外的所有场景
    private IEnumerator UnloadAllExcept(Scene keepScene)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene != keepScene && scene.isLoaded)
            {
                Debug.Log($"卸载场景: {scene.name}");
                yield return SceneManager.UnloadSceneAsync(scene);
            }
        }
    }
}
