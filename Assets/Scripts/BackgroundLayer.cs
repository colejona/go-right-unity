using UnityEngine;

public class BackgroundLayer : MonoBehaviour
{
    [SerializeField] float parallaxFactor = 0.5f;
    [SerializeField] int tileCount = 7;
    [SerializeField] float tileWidth = 20f;
    [SerializeField] Color colorA = new Color(0.15f, 0.15f, 0.25f);
    [SerializeField] Color colorB = new Color(0.20f, 0.20f, 0.32f);

    BackgroundTileLogic _logic;
    Transform[] _tiles;
    Transform _cameraTransform;

    void Awake()
    {
        _logic = new BackgroundTileLogic(parallaxFactor, tileWidth, tileCount);
        _tiles = new Transform[tileCount];

        for (int i = 0; i < tileCount; i++)
        {
            var go = new GameObject($"Tile_{i}");
            go.transform.SetParent(transform);

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = CreateStripeSprite(i % 2 == 0 ? colorA : colorB);
            sr.sortingOrder = -10;

            _tiles[i] = go.transform;
        }
    }

    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        float camX = _cameraTransform.position.x;
        float[] positions = _logic.GetTilePositions(camX);

        for (int i = 0; i < _tiles.Length; i++)
        {
            var pos = _tiles[i].position;
            pos.x = positions[i] + tileWidth * 0.5f;
            pos.y = 0f;
            _tiles[i].position = pos;
        }
    }

    static Sprite CreateStripeSprite(Color color)
    {
        const int w = 64, h = 64;
        var tex = new Texture2D(w, h);
        var pixels = new Color[w * h];

        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                pixels[y * w + x] = (x / 8) % 2 == 0 ? color : color * 0.85f;

        tex.SetPixels(pixels);
        tex.Apply();
        tex.filterMode = FilterMode.Point;

        return Sprite.Create(tex, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f), w / tileWidth_PPU);
    }

    const float tileWidth_PPU = 3.2f; // pixels per unit: 64px / 20 units
}
