using UnityEngine;

public class FloorLayer : MonoBehaviour
{
    [SerializeField] int tileCount = 30;
    [SerializeField] float tileWidth = 2f;
    [SerializeField] float tileHeight = 0.5f;
    [SerializeField] Color colorA = new Color(0.45f, 0.42f, 0.38f);
    [SerializeField] Color colorB = new Color(0.38f, 0.35f, 0.30f);

    BackgroundTileLogic _logic;
    Transform[] _tiles;
    SpriteRenderer[] _renderers;
    Sprite _spriteA;
    Sprite _spriteB;
    Transform _cameraTransform;

    void Awake()
    {
        _logic = new BackgroundTileLogic(0f, tileWidth, tileCount);
        _tiles = new Transform[tileCount];
        _renderers = new SpriteRenderer[tileCount];
        _spriteA = CreateTileSprite(colorA);
        _spriteB = CreateTileSprite(colorB);

        for (int i = 0; i < tileCount; i++)
        {
            var go = new GameObject($"FloorTile_{i}");
            go.transform.SetParent(transform);

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sortingOrder = 1;

            _tiles[i] = go.transform;
            _renderers[i] = sr;
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
            _tiles[i].position = new Vector3(
                positions[i] + tileWidth * 0.5f,
                transform.position.y,
                0f
            );
            _renderers[i].sprite = BackgroundTileLogic.TileColorIndex(positions[i], tileWidth) == 0 ? _spriteA : _spriteB;
        }
    }

    Sprite CreateTileSprite(Color color)
    {
        int w = 32;
        int h = Mathf.Max(1, Mathf.RoundToInt(w * tileHeight / tileWidth));
        var tex = new Texture2D(w, h);
        var pixels = new Color[w * h];

        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                bool isEdge = x == 0 || x == w - 1 || y == 0 || y == h - 1;
                pixels[y * w + x] = isEdge ? color * 1.4f : color;
            }

        tex.SetPixels(pixels);
        tex.Apply();
        tex.filterMode = FilterMode.Point;

        float ppu = w / tileWidth;
        return Sprite.Create(tex, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f), ppu);
    }
}
