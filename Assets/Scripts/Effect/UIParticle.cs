using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIParticle : MonoBehaviour
{
    public enum ParticleMode
    {
        Up,
        Down,
        Explosion,
        Random
    }

    public enum EmitMode
    {
        Continuous,
        Burst
    }

    [Header("Particle")]
    public Sprite particleSprite;

    [Range(1, 1000)]
    public int maxParticleCount = 200;

    public float speed = 100f;
    public float lifeTime = 2f;
    public ParticleMode moveMode = ParticleMode.Up;

    [Header("Size")]
    public bool keepAspectRatio = true;
    public Vector2 widthRange = new Vector2(10, 30);
    public Vector2 heightRange = new Vector2(10, 30);

    [Header("Emit Mode")]
    public EmitMode emitMode = EmitMode.Continuous;

    [Tooltip("连续生成时，每秒生成多少个粒子")]
    public float emissionRate = 30f;

    [Tooltip("Burst模式下，每波生成多少个粒子")]
    public int burstCount = 20;

    [Tooltip("Burst模式下，每波间隔时间")]
    public float burstInterval = 1f;

    [Header("Visible Area")]
    [Tooltip("粒子的可见区域。超出这个 RectTransform 就会消失。为空则不检测区域。")]
    public RectTransform visibleArea;

    public bool destroyWhenOutsideArea = true;

    private class Particle
    {
        public RectTransform rect;
        public Image image;
        public Vector2 velocity;
        public float timer;
        public float maxLife;
        public bool active;
    }

    private readonly List<Particle> particles = new();

    private RectTransform rectTransform;
    private float continuousTimer;
    private float burstTimer;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        CreateParticlePool();
    }

    void Update()
    {
        EmitUpdate();
        ParticleUpdate();
    }

    void CreateParticlePool()
    {
        for (int i = 0; i < maxParticleCount; i++)
        {
            GameObject obj = new GameObject("UI Particle", typeof(Image));
            obj.transform.SetParent(transform, false);

            Image img = obj.GetComponent<Image>();
            img.sprite = particleSprite;
            img.raycastTarget = false;

            RectTransform rt = obj.GetComponent<RectTransform>();

            Particle p = new Particle
            {
                rect = rt,
                image = img,
                active = false
            };

            obj.SetActive(false);
            particles.Add(p);
        }
    }

    void EmitUpdate()
    {
        if (emitMode == EmitMode.Continuous)
        {
            continuousTimer += Time.deltaTime * emissionRate;

            while (continuousTimer >= 1f)
            {
                EmitOne();
                continuousTimer -= 1f;
            }
        }
        else
        {
            burstTimer += Time.deltaTime;

            if (burstTimer >= burstInterval)
            {
                EmitBurst();
                burstTimer = 0f;
            }
        }
    }

    void EmitBurst()
    {
        for (int i = 0; i < burstCount; i++)
        {
            EmitOne();
        }
    }

    void EmitOne()
    {
        Particle p = GetInactiveParticle();

        if (p == null)
            return;

        SpawnParticle(p);
    }

    Particle GetInactiveParticle()
    {
        foreach (var p in particles)
        {
            if (!p.active)
                return p;
        }

        return null;
    }

    void SpawnParticle(Particle p)
    {
        p.active = true;
        p.timer = 0f;
        p.maxLife = lifeTime;

        p.rect.gameObject.SetActive(true);
        p.rect.anchoredPosition = GetRandomPositionInRect();

        SetParticleSize(p);

        Vector2 dir = GetMoveDirection(p.rect.anchoredPosition);

        p.velocity =
            dir *
            Random.Range(speed * 0.5f, speed);

        Color c = p.image.color;
        c.a = 1f;
        p.image.color = c;
    }

    void SetParticleSize(Particle p)
    {
        if (keepAspectRatio)
        {
            float size = Random.Range(widthRange.x, widthRange.y);
            p.rect.sizeDelta = new Vector2(size, size);
        }
        else
        {
            float width = Random.Range(widthRange.x, widthRange.y);
            float height = Random.Range(heightRange.x, heightRange.y);
            p.rect.sizeDelta = new Vector2(width, height);
        }
    }

    void ParticleUpdate()
    {
        float dt = Time.deltaTime;

        foreach (var p in particles)
        {
            if (!p.active)
                continue;

            p.timer += dt;
            p.rect.anchoredPosition += p.velocity * dt;

            float alpha = 1f - p.timer / p.maxLife;
            alpha = Mathf.Clamp01(alpha);

            Color c = p.image.color;
            c.a = alpha;
            p.image.color = c;

            if (p.timer >= p.maxLife)
            {
                DisableParticle(p);
                continue;
            }

            if (destroyWhenOutsideArea && !IsInsideVisibleArea(p.rect))
            {
                DisableParticle(p);
            }
        }
    }

    bool IsInsideVisibleArea(RectTransform particleRect)
    {
        if (visibleArea == null)
            return true;

        Vector3 worldPos = particleRect.position;

        Vector2 localPos =
            visibleArea.InverseTransformPoint(worldPos);

        return visibleArea.rect.Contains(localPos);
    }

    void DisableParticle(Particle p)
    {
        p.active = false;
        p.rect.gameObject.SetActive(false);
    }

    Vector2 GetRandomPositionInRect()
    {
        Rect rect = rectTransform.rect;

        float x = Random.Range(rect.xMin, rect.xMax);
        float y = Random.Range(rect.yMin, rect.yMax);

        return new Vector2(x, y);
    }

    Vector2 GetMoveDirection(Vector2 spawnPos)
    {
        Vector2 dir = Vector2.up;

        switch (moveMode)
        {
            case ParticleMode.Up:
                dir = Vector2.up;
                break;

            case ParticleMode.Down:
                dir = Vector2.down;
                break;

            case ParticleMode.Explosion:
                dir = spawnPos.normalized;

                if (dir == Vector2.zero)
                    dir = Random.insideUnitCircle.normalized;
                break;

            case ParticleMode.Random:
                dir = Random.insideUnitCircle.normalized;
                break;
        }

        return dir.normalized;
    }
}