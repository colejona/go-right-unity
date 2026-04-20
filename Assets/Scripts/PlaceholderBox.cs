using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlaceholderBox : MonoBehaviour
{
    [SerializeField] Color color = Color.white;

    void Awake()
    {
        var tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        var sprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
        var sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = color;
    }
}
