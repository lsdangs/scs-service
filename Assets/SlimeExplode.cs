using UnityEngine;

public class SlimeExplode : MonoBehaviour
{
    [Header("폭발 설정")]
    [Tooltip("폭발 반경. 이 안에 있는 슬라임이 연쇄로 터짐")]
    public float explodeRadius = 2f;

    [Tooltip("폭발 범위 미리보기 원 색")]
    public Color previewColor = new Color(1f, 0.5f, 0f, 0.3f);

    private bool exploded = false;
    private SpriteRenderer previewCircle;

    void OnMouseEnter()
    {
        ShowPreview(true);
    }

    void OnMouseExit()
    {
        ShowPreview(false);
    }

    void OnMouseDown()
    {
        Explode();
    }

    void ShowPreview(bool on)
    {
        if (exploded) return;

        if (on && previewCircle == null)
        {
            GameObject circle = new GameObject("ExplodePreview");
            circle.transform.SetParent(transform);
            circle.transform.localPosition = Vector3.zero;

            previewCircle = circle.AddComponent<SpriteRenderer>();
            previewCircle.sprite = MakeCircleSprite();
            previewCircle.color = previewColor;
            previewCircle.sortingOrder = 5;

            float diameter = explodeRadius * 2f;
            circle.transform.localScale = new Vector3(diameter, diameter, 1f);
        }

        if (previewCircle != null)
            previewCircle.enabled = on;
    }

    public void Explode()
    {
        if (exploded) return;
        exploded = true;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explodeRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            // 다른 슬라임이면 연쇄 폭발
            SlimeExplode other = hit.GetComponent<SlimeExplode>();
            if (other != null && !other.exploded)
            {
                other.Explode();
            }

            // 목표물이면 파괴
            Target target = hit.GetComponent<Target>();
            if (target != null)
            {
                target.DestroyTarget();
            }
        }

        Debug.Log("폭발! 위치: " + transform.position);

        if (previewCircle != null) Destroy(previewCircle.gameObject);
        Destroy(gameObject);
    }

    Sprite MakeCircleSprite()
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size);
        tex.filterMode = FilterMode.Bilinear;
        float r = size / 2f;
        Vector2 center = new Vector2(r, r);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), center);
                Color c = dist <= r ? Color.white : Color.clear;
                tex.SetPixel(x, y, c);
            }
        }
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
    }
}