using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// UI 点击 + 拖拽 + 编辑器自动生成粒子系统
/// 同时支持：
/// 1. 单方向随机喷射
/// 2. 四面八方喷射
///
/// 适合：树叶、花瓣、纸片等可拖拽 UI 散落效果
/// 
/// 本版本改进：
/// 1. 拖拽移动改为基于世界坐标计算，而不是 anchoredPosition 局部坐标
/// 2. 拖拽粒子方向改为优先使用世界坐标拖拽方向
/// 3. 对 Screen Space Overlay / Screen Space Camera / World Space Canvas 更稳定
/// </summary>
[ExecuteAlways]
[DisallowMultipleComponent]
public class Effect_Drag : MonoBehaviour,
    IPointerDownHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    public enum DirectionalEmitMode
    {
        Radial,
        UpwardCone,
        DownwardCone,
        DragOpposite,
        DragSameDirection,
        CustomDirection
    }

    [Header("Visual")]
    public Image targetImage;
    public Sprite[] stateSprites;

    [Header("Scale")]
    public float scaleMultiplier = 1.1f;
    public float clickScaleMultiplier = 1.1f;
    public float scaleDuration = 0.15f;

    [Header("Canvas / Drag")]
    public bool autoFindCanvas = true;

    [Header("Particle Auto Build")]
    public bool autoCreateParticleSystem = true;
    public string particleChildName = "LeafScatterPS";
    public bool autoRebuildInEditor = true;

    [Header("Particle Render")]
    public Sprite particleSprite;
    public string particleShaderName = "Sprites/Default";
    public string sortingLayerName = "Default";
    public int sortingOrder = 50;
    public Vector3 particleLocalOffset = Vector3.zero;

    [Header("Particle Trigger")]
    public bool enableParticle = true;
    public bool emitOnClick = true;
    public bool emitWhileDragging = true;
    public bool emitOnEndDrag = false;

    [Range(0, 100)] public int clickBurstCount = 8;
    [Range(0, 100)] public int endDragBurstCount = 6;
    [Range(1, 30)] public int dragEmitCount = 2;
    [Range(0.01f, 0.3f)] public float dragEmitInterval = 0.05f;
    [Range(0f, 50f)] public float dragDistanceThreshold = 3f;

    [Header("Directional Emit (单方向随机喷射)")]
    public bool enableDirectionalBurst = true;
    public DirectionalEmitMode directionalEmitMode = DirectionalEmitMode.DragOpposite;
    [Range(0f, 360f)] public float directionalAngleSpread = 45f;
    public Vector2 customDirection = new Vector2(0f, 1f);
    [Range(0f, 5f)] public float dragVelocityInfluence = 0.8f;

    [Header("Radial Emit (四面八方喷射)")]
    public bool enableRadialBurst = true;
    public bool radialEvenSpread = true;
    [Range(0f, 3f)] public float radialTangentJitter = 0.35f;
    [Range(0f, 3f)] public float radialBurstBoost = 1.0f;

    [Header("Burst Mix Ratio")]
    [Tooltip("点击时，单方向喷射占比。0=全四向，1=全单方向")]
    [Range(0f, 1f)] public float clickDirectionalRatio = 0.35f;

    [Tooltip("拖拽时，单方向喷射占比。0=全四向，1=全单方向")]
    [Range(0f, 1f)] public float dragDirectionalRatio = 0.8f;

    [Tooltip("结束拖拽时，单方向喷射占比。0=全四向，1=全单方向")]
    [Range(0f, 1f)] public float endDragDirectionalRatio = 0.25f;

    [Header("Particle Motion")]
    [Range(0.1f, 5f)] public float particleLifetime = 1.2f;
    [Range(0f, 10f)] public float startSpeed = 1.4f;
    [Range(0f, 5f)] public float startSpeedRandom = 0.8f;
    [Range(0.01f, 1f)] public float startSize = 0.12f;
    [Range(0f, 1f)] public float startSizeRandom = 0.05f;
    [Range(-5f, 10f)] public float gravityModifier = 0.6f;

    [Header("Particle Rotation")]
    public bool enableRotationOverLifetime = true;
    [Range(0f, 360f)] public float startRotationRandom = 180f;
    public float angularVelocityMin = -120f;
    public float angularVelocityMax = 120f;

    [Header("Particle Appearance")]
    public Color particleColor = new Color(0.75f, 0.95f, 0.45f, 0.95f);
    public bool useFadeOut = true;

    [Header("Follow")]
    public bool particleFollowTarget = true;

    [Header("Debug")]
    public bool logWarnings = false;

    private RectTransform rectTransform;
    private Canvas canvas;

    private int currentState = 0;
    private Vector3 originalScale;
    private bool isDragging = false;

    private ParticleSystem runtimeParticle;
    private ParticleSystemRenderer runtimeParticleRenderer;
    private Material runtimeParticleMaterial;

    private float dragEmitTimer = 0f;
    private Vector2 lastDragScreenPosition;
    private Vector2 lastDragDelta;

    // 新增：世界坐标拖拽缓存
    private Vector3 lastDragWorldPosition;
    private bool hasLastDragWorldPosition = false;

    private bool isApplyingParticleSettings = false;

    private enum EmitContext
    {
        Click,
        Drag,
        EndDrag
    }

    void Reset()
    {
        rectTransform = GetComponent<RectTransform>();

        if (targetImage == null)
            targetImage = GetComponent<Image>();

        if (autoFindCanvas)
            canvas = GetComponentInParent<Canvas>();

        if (autoCreateParticleSystem)
        {
            EnsureParticleSystemExistsInEditor();
            SafeApplyParticleSettings();
        }
    }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (targetImage == null)
            targetImage = GetComponent<Image>();

        if (autoFindCanvas || canvas == null)
            canvas = GetComponentInParent<Canvas>();

        if (Application.isPlaying && autoCreateParticleSystem)
        {
            EnsureParticleSystemReferenceOnly();
            SafeApplyParticleSettings();
        }
    }

    void Start()
    {
        originalScale = transform.localScale;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (!isDragging)
            {
                StopAllCoroutines();
                StartCoroutine(ClickUpScale());
            }
        }

        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        if ((autoFindCanvas && canvas == null) || canvas == null)
            canvas = GetComponentInParent<Canvas>();

        if (particleFollowTarget && runtimeParticle != null)
        {
            runtimeParticle.transform.localPosition = particleLocalOffset;
            runtimeParticle.transform.localRotation = Quaternion.identity;
        }

#if UNITY_EDITOR
        if (!Application.isPlaying && autoCreateParticleSystem && autoRebuildInEditor)
        {
            EnsureParticleSystemExistsInEditor();
        }
#endif
    }

    void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(particleChildName))
            particleChildName = "LeafScatterPS";

        scaleMultiplier = Mathf.Max(0.01f, scaleMultiplier);
        clickScaleMultiplier = Mathf.Max(0.01f, clickScaleMultiplier);
        scaleDuration = Mathf.Max(0.01f, scaleDuration);

        particleLifetime = Mathf.Max(0.05f, particleLifetime);
        startSpeed = Mathf.Max(0f, startSpeed);
        startSpeedRandom = Mathf.Max(0f, startSpeedRandom);
        startSize = Mathf.Max(0.001f, startSize);
        startSizeRandom = Mathf.Max(0f, startSizeRandom);
        dragEmitInterval = Mathf.Max(0.01f, dragEmitInterval);
        dragEmitCount = Mathf.Max(1, dragEmitCount);

        if (customDirection.sqrMagnitude < 0.0001f)
            customDirection = Vector2.up;

#if UNITY_EDITOR
        if (!Application.isPlaying && autoCreateParticleSystem)
        {
            EnsureParticleSystemExistsInEditor();
            SafeApplyParticleSettings();
            MarkEditorDirty();
            return;
        }
#endif

        if (Application.isPlaying)
        {
            EnsureParticleSystemReferenceOnly();
            SafeApplyParticleSettings();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayClickParticle();
        SwitchSprite();

        if (!isDragging)
        {
            StopAllCoroutines();
            StartCoroutine(ClickScale());
        }

        lastDragScreenPosition = eventData.position;
        lastDragDelta = Vector2.zero;
        hasLastDragWorldPosition = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        dragEmitTimer = 0f;
        lastDragScreenPosition = eventData.position;
        lastDragDelta = Vector2.zero;

        StopAllCoroutines();
        transform.localScale = originalScale * scaleMultiplier;

        hasLastDragWorldPosition = TryGetPointerWorldPosition(eventData, out lastDragWorldPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        if ((autoFindCanvas && canvas == null) || canvas == null)
            canvas = GetComponentInParent<Canvas>();

        if (rectTransform == null || canvas == null)
            return;

        Vector2 particleDragDelta = Vector2.zero;

        Vector3 currentWorldPosition;
        if (TryGetPointerWorldPosition(eventData, out currentWorldPosition))
        {
            if (!hasLastDragWorldPosition)
            {
                lastDragWorldPosition = currentWorldPosition;
                hasLastDragWorldPosition = true;
            }

            Vector3 worldDelta = currentWorldPosition - lastDragWorldPosition;
            rectTransform.position += worldDelta;

            particleDragDelta = new Vector2(worldDelta.x, worldDelta.y);
            lastDragWorldPosition = currentWorldPosition;
        }
        else
        {
            // 回退：如果某些情况下无法转换世界坐标，则继续使用屏幕位移估算粒子方向
            particleDragDelta = eventData.position - lastDragScreenPosition;
        }

        lastDragDelta = particleDragDelta;
        lastDragScreenPosition = eventData.position;

        if (enableParticle && emitWhileDragging)
        {
            dragEmitTimer += Time.unscaledDeltaTime;

            if (dragEmitTimer >= dragEmitInterval && lastDragDelta.magnitude >= dragDistanceThreshold)
            {
                EmitMixedBurst(dragEmitCount, lastDragDelta, EmitContext.Drag);
                dragEmitTimer = 0f;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        hasLastDragWorldPosition = false;
        transform.localScale = originalScale;

        if (enableParticle && emitOnEndDrag)
            EmitMixedBurst(endDragBurstCount, lastDragDelta, EmitContext.EndDrag);
    }

    IEnumerator ClickScale()
    {
        Vector3 target = originalScale * clickScaleMultiplier;
        float time = 0f;

        while (time < scaleDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, target, time / scaleDuration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localScale = target;
    }

    IEnumerator ClickUpScale()
    {
        Vector3 target = transform.localScale;
        float time = 0f;

        while (time < scaleDuration)
        {
            transform.localScale = Vector3.Lerp(target, originalScale, time / scaleDuration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
    }

    void SwitchSprite()
    {
        if (targetImage == null) return;
        if (stateSprites == null || stateSprites.Length == 0) return;

        currentState = (currentState + 1) % stateSprites.Length;
        targetImage.sprite = stateSprites[currentState];
        targetImage.SetAllDirty();
    }

    void PlayClickParticle()
    {
        if (!enableParticle || !emitOnClick) return;
        EmitMixedBurst(clickBurstCount, Vector2.zero, EmitContext.Click);
    }

    void EmitMixedBurst(int count, Vector2 dragDelta, EmitContext context)
    {
        if (!enableParticle || count <= 0)
            return;

        EnsureParticleSystemReferenceOnly();
        if (runtimeParticle == null)
            return;

        float directionalRatio = GetDirectionalRatio(context);

        int directionalCount = 0;
        int radialCount = 0;

        if (enableDirectionalBurst && enableRadialBurst)
        {
            directionalCount = Mathf.RoundToInt(count * directionalRatio);
            directionalCount = Mathf.Clamp(directionalCount, 0, count);
            radialCount = count - directionalCount;
        }
        else if (enableDirectionalBurst)
        {
            directionalCount = count;
            radialCount = 0;
        }
        else if (enableRadialBurst)
        {
            directionalCount = 0;
            radialCount = count;
        }
        else
        {
            return;
        }

        if (directionalCount > 0)
            EmitDirectionalBurst(directionalCount, dragDelta);

        if (radialCount > 0)
            EmitRadialBurst(radialCount, dragDelta);

        if (!runtimeParticle.isPlaying)
            runtimeParticle.Play();
    }

    float GetDirectionalRatio(EmitContext context)
    {
        switch (context)
        {
            case EmitContext.Click:
                return clickDirectionalRatio;
            case EmitContext.Drag:
                return dragDirectionalRatio;
            case EmitContext.EndDrag:
                return endDragDirectionalRatio;
        }
        return 0.5f;
    }

    void EmitDirectionalBurst(int count, Vector2 dragDelta)
    {
        Vector2 baseDir = ResolveDirectionalBaseDirection(dragDelta);

        for (int i = 0; i < count; i++)
        {
            ParticleSystem.EmitParams ep = CreateBaseEmitParams();

            float angleOffset = Random.Range(-directionalAngleSpread * 0.5f, directionalAngleSpread * 0.5f);
            Vector2 dir = Rotate2D(baseDir, angleOffset).normalized;

            float speed = Random.Range(
                Mathf.Max(0f, startSpeed - startSpeedRandom),
                startSpeed + startSpeedRandom
            );

            Vector3 velocity = new Vector3(dir.x, dir.y, 0f) * speed;

            if (dragDelta.sqrMagnitude > 0.0001f &&
                (directionalEmitMode == DirectionalEmitMode.DragOpposite || directionalEmitMode == DirectionalEmitMode.DragSameDirection))
            {
                Vector2 dragDir = dragDelta.normalized;
                Vector2 extra = dragDir * dragVelocityInfluence * Random.Range(0.6f, 1.2f);

                if (directionalEmitMode == DirectionalEmitMode.DragOpposite)
                    extra = -extra;

                velocity += new Vector3(extra.x, extra.y, 0f);
            }

            ep.velocity = velocity;
            runtimeParticle.Emit(ep, 1);
        }
    }

    void EmitRadialBurst(int count, Vector2 dragDelta)
    {
        float angleStep = count > 0 ? 360f / count : 360f;
        float baseAngleOffset = Random.Range(0f, 360f);

        for (int i = 0; i < count; i++)
        {
            ParticleSystem.EmitParams ep = CreateBaseEmitParams();

            float angle;
            if (radialEvenSpread)
            {
                angle = baseAngleOffset + angleStep * i + Random.Range(-angleStep * 0.25f, angleStep * 0.25f);
            }
            else
            {
                angle = Random.Range(0f, 360f);
            }

            Vector2 dir = AngleToDirection(angle);

            float speed = Random.Range(
                Mathf.Max(0f, startSpeed - startSpeedRandom),
                startSpeed + startSpeedRandom
            ) * Mathf.Max(0.01f, radialBurstBoost);

            Vector2 tangent = new Vector2(-dir.y, dir.x) * Random.Range(-radialTangentJitter, radialTangentJitter);

            Vector2 dragInfluence = Vector2.zero;
            if (dragDelta.sqrMagnitude > 0.0001f)
            {
                Vector2 dragDir = dragDelta.normalized;
                dragInfluence = dragDir * dragVelocityInfluence * Random.Range(0.15f, 0.5f);
            }

            Vector3 velocity = new Vector3(
                dir.x + tangent.x + dragInfluence.x,
                dir.y + tangent.y + dragInfluence.y,
                0f
            );

            if (velocity.sqrMagnitude < 0.0001f)
                velocity = new Vector3(dir.x, dir.y, 0f);

            velocity = velocity.normalized * speed;

            ep.velocity = velocity;
            runtimeParticle.Emit(ep, 1);
        }
    }

    ParticleSystem.EmitParams CreateBaseEmitParams()
    {
        ParticleSystem.EmitParams ep = new ParticleSystem.EmitParams();
        ep.position = Vector3.zero;
        ep.startLifetime = particleLifetime * Random.Range(0.85f, 1.15f);
        ep.startSize = Random.Range(
            Mathf.Max(0.001f, startSize - startSizeRandom),
            startSize + startSizeRandom
        );
        ep.startColor = particleColor;
        ep.rotation = Random.Range(0f, Mathf.Max(0f, startRotationRandom)) * Mathf.Deg2Rad;
        ep.angularVelocity = Random.Range(angularVelocityMin, angularVelocityMax) * Mathf.Deg2Rad;
        return ep;
    }

    Vector2 ResolveDirectionalBaseDirection(Vector2 dragDelta)
    {
        switch (directionalEmitMode)
        {
            case DirectionalEmitMode.Radial:
                {
                    Vector2 random = Random.insideUnitCircle;
                    return random.sqrMagnitude > 0.0001f ? random.normalized : Vector2.up;
                }

            case DirectionalEmitMode.UpwardCone:
                return Vector2.up;

            case DirectionalEmitMode.DownwardCone:
                return Vector2.down;

            case DirectionalEmitMode.DragOpposite:
                if (dragDelta.sqrMagnitude > 0.0001f)
                    return (-dragDelta).normalized;
                return Vector2.up;

            case DirectionalEmitMode.DragSameDirection:
                if (dragDelta.sqrMagnitude > 0.0001f)
                    return dragDelta.normalized;
                return Vector2.up;

            case DirectionalEmitMode.CustomDirection:
                return customDirection.normalized;
        }

        return Vector2.up;
    }

    Vector2 Rotate2D(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }

    Vector2 AngleToDirection(float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
    }

    /// <summary>
    /// 将指针屏幕坐标转换为 UI 所在平面上的世界坐标
    /// </summary>
    bool TryGetPointerWorldPosition(PointerEventData eventData, out Vector3 worldPos)
    {
        worldPos = Vector3.zero;

        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        if ((autoFindCanvas && canvas == null) || canvas == null)
            canvas = GetComponentInParent<Canvas>();

        if (rectTransform == null || canvas == null)
            return false;

        Camera cam = eventData.pressEventCamera;

        RectTransform referenceRect = canvas.transform as RectTransform;
        if (referenceRect == null)
            referenceRect = rectTransform;

        return RectTransformUtility.ScreenPointToWorldPointInRectangle(
            referenceRect,
            eventData.position,
            cam,
            out worldPos
        );
    }

    void EnsureParticleSystemReferenceOnly()
    {
        if (runtimeParticle != null) return;

        Transform child = transform.Find(particleChildName);
        if (child == null) return;

        runtimeParticle = child.GetComponent<ParticleSystem>();
        runtimeParticleRenderer = child.GetComponent<ParticleSystemRenderer>();

        if (runtimeParticle != null && runtimeParticleRenderer == null)
            runtimeParticleRenderer = child.GetComponent<ParticleSystemRenderer>();
    }

#if UNITY_EDITOR
    void EnsureParticleSystemExistsInEditor()
    {
        EnsureParticleSystemReferenceOnly();

        if (runtimeParticle != null)
            return;

        Transform child = transform.Find(particleChildName);
        GameObject go = child != null ? child.gameObject : null;

        if (go == null)
        {
            go = new GameObject(particleChildName);
            Undo.RegisterCreatedObjectUndo(go, "Create Leaf Particle System");
            go.transform.SetParent(transform, false);
        }

        go.transform.localPosition = particleLocalOffset;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;

        runtimeParticle = go.GetComponent<ParticleSystem>();
        if (runtimeParticle == null)
            runtimeParticle = Undo.AddComponent<ParticleSystem>(go);

        runtimeParticleRenderer = go.GetComponent<ParticleSystemRenderer>();
        if (runtimeParticleRenderer == null)
            runtimeParticleRenderer = Undo.AddComponent<ParticleSystemRenderer>(go);

        CreateOrAssignParticleMaterial();
        ForceStopParticle();
    }
#endif

    void CreateOrAssignParticleMaterial()
    {
        if (runtimeParticleRenderer == null)
            return;

        Shader shader = Shader.Find(particleShaderName);
        if (shader == null)
        {
            shader = Shader.Find("Sprites/Default");
            if (logWarnings)
                Debug.LogWarning($"未找到 Shader: {particleShaderName}，已回退到 Sprites/Default", this);
        }

        if (shader == null)
            return;

        bool needCreate = runtimeParticleMaterial == null || runtimeParticleMaterial.shader != shader;
        if (needCreate)
        {
            runtimeParticleMaterial = new Material(shader);
            runtimeParticleMaterial.name = $"{name}_LeafParticleMat";
#if UNITY_EDITOR
            runtimeParticleMaterial.hideFlags = HideFlags.DontSaveInEditor;
#endif
        }

        if (particleSprite != null && particleSprite.texture != null)
            runtimeParticleMaterial.mainTexture = particleSprite.texture;

        runtimeParticleRenderer.sharedMaterial = runtimeParticleMaterial;
    }

    void ForceStopParticle()
    {
        if (runtimeParticle == null) return;

        runtimeParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        runtimeParticle.Clear(true);
    }

    void SafeApplyParticleSettings()
    {
        if (runtimeParticle == null)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying && autoCreateParticleSystem)
                EnsureParticleSystemExistsInEditor();
            else
#endif
                EnsureParticleSystemReferenceOnly();
        }

        if (runtimeParticle == null) return;
        if (isApplyingParticleSettings) return;

        isApplyingParticleSettings = true;

        ForceStopParticle();
        ApplyParticleSettingsInternal();

        isApplyingParticleSettings = false;
    }

    void ApplyParticleSettingsInternal()
    {
        if (runtimeParticle == null) return;

        var main = runtimeParticle.main;
        main.loop = false;
        main.playOnAwake = false;
        main.duration = Mathf.Max(0.1f, particleLifetime);
        main.startLifetime = particleLifetime;
        main.startSpeed = 0f;
        main.startSize = startSize;
        main.startColor = particleColor;
        main.gravityModifier = gravityModifier;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.scalingMode = ParticleSystemScalingMode.Local;
        main.maxParticles = 500;
        main.startRotation = 0f;

        var emission = runtimeParticle.emission;
        emission.enabled = false;
        emission.rateOverTime = 0f;
        emission.rateOverDistance = 0f;

        var shape = runtimeParticle.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.02f;
        shape.radiusThickness = 1f;
        shape.arcMode = ParticleSystemShapeMultiModeValue.Random;
        shape.arc = 360f;

        var velocityOverLifetime = runtimeParticle.velocityOverLifetime;
        velocityOverLifetime.enabled = false;

        var limitVelocity = runtimeParticle.limitVelocityOverLifetime;
        limitVelocity.enabled = false;

        var noise = runtimeParticle.noise;
        noise.enabled = false;

        var colorOverLifetime = runtimeParticle.colorOverLifetime;
        colorOverLifetime.enabled = useFadeOut;
        if (useFadeOut)
        {
            Gradient g = new Gradient();
            g.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(Color.white, 0f),
                    new GradientColorKey(Color.white, 1f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(0.9f, 0.65f),
                    new GradientAlphaKey(0f, 1f)
                }
            );
            colorOverLifetime.color = new ParticleSystem.MinMaxGradient(g);
        }

        var sizeOverLifetime = runtimeParticle.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve sizeCurve = new AnimationCurve(
            new Keyframe(0f, 1f),
            new Keyframe(0.75f, 0.95f),
            new Keyframe(1f, 0.6f)
        );
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);

        var rotationOverLifetime = runtimeParticle.rotationOverLifetime;
        rotationOverLifetime.enabled = enableRotationOverLifetime;
        if (enableRotationOverLifetime)
        {
            rotationOverLifetime.z = new ParticleSystem.MinMaxCurve(
                Mathf.Deg2Rad * angularVelocityMin,
                Mathf.Deg2Rad * angularVelocityMax
            );
        }

        if (runtimeParticleRenderer != null)
        {
            CreateOrAssignParticleMaterial();
            runtimeParticleRenderer.renderMode = ParticleSystemRenderMode.Billboard;
            runtimeParticleRenderer.alignment = ParticleSystemRenderSpace.View;
            runtimeParticleRenderer.sortingLayerName = sortingLayerName;
            runtimeParticleRenderer.sortingOrder = sortingOrder;
            runtimeParticleRenderer.sharedMaterial = runtimeParticleMaterial;
        }

        runtimeParticle.transform.localPosition = particleLocalOffset;
        runtimeParticle.transform.localRotation = Quaternion.identity;
        runtimeParticle.transform.localScale = Vector3.one;
    }

    [ContextMenu("Rebuild Particle System Now")]
    public void RebuildParticleSystem()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            EnsureParticleSystemExistsInEditor();
        else
            EnsureParticleSystemReferenceOnly();
#else
        EnsureParticleSystemReferenceOnly();
#endif
        SafeApplyParticleSettings();
    }

    [ContextMenu("Preview Click Emit")]
    public void PreviewClickEmit()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            EnsureParticleSystemExistsInEditor();
            SafeApplyParticleSettings();
        }
#endif
        EmitMixedBurst(clickBurstCount, Vector2.zero, EmitContext.Click);
    }

    public void ResetItem()
    {
        currentState = 0;

        if (targetImage != null && stateSprites != null && stateSprites.Length > 0)
            targetImage.sprite = stateSprites[0];

        transform.localScale = originalScale;

        hasLastDragWorldPosition = false;
        lastDragDelta = Vector2.zero;
        lastDragScreenPosition = Vector2.zero;

        if (runtimeParticle != null)
            ForceStopParticle();
    }

#if UNITY_EDITOR
    void MarkEditorDirty()
    {
        if (this != null)
            EditorUtility.SetDirty(this);

        if (runtimeParticle != null)
            EditorUtility.SetDirty(runtimeParticle);

        if (runtimeParticleRenderer != null)
            EditorUtility.SetDirty(runtimeParticleRenderer);

        if (gameObject != null)
            EditorUtility.SetDirty(gameObject);
    }
#endif
}