using UnityEngine;

public class MpBar : MonoBehaviour
{
    [SerializeField] float width = 1.6f;
    [SerializeField] float height = 0.12f;
    [SerializeField] float yOffset = 0.98f;

    static readonly Color FillColor = new Color(0.2f, 0.4f, 0.9f);

    SpriteRenderer _bg;
    SpriteRenderer _fill;

    void Awake()
    {
        _bg   = CreateBar("MpBar_BG",   new Color(0.2f, 0.2f, 0.2f), 0);
        _fill = CreateBar("MpBar_Fill", FillColor,                    1);
    }

    SpriteRenderer CreateBar(string goName, Color color, int sortingOffset)
    {
        var go = new GameObject(goName);
        go.transform.SetParent(transform, false);
        go.transform.localPosition = new Vector3(0f, yOffset, 0f);

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = CreatePixelSprite();
        sr.color = color;
        sr.sortingOrder = 5 + sortingOffset;
        go.transform.localScale = new Vector3(width, height, 1f);

        return sr;
    }

    static Sprite CreatePixelSprite()
    {
        var tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
    }

    public void Refresh(int currentMp, int maxMp)
    {
        float fraction = HpBarLogic.FillFraction(currentMp, maxMp);
        _fill.transform.localScale = new Vector3(width * fraction, height, 1f);
        _fill.transform.localPosition = new Vector3(-width * (1f - fraction) * 0.5f, yOffset, 0f);
    }
}
